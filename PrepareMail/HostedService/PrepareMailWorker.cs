using System.Collections.Immutable;
using System.Text.Json;
using Common.HttpService;
using Common.MessageBroker.Consumer;
using Common.MessageBroker.Consumer.Interfaces;
using Common.MessageBroker.Consumer.Utilities;
using Common.MessageBroker.Publisher.Interfaces;
using Common.Messages.EmailMessages;
using Common.Messages.EmailMessages.Interfaces;
using Common.Messages.PublishSubscribeMessages;
using Common.XmlService;
using PrepareMail.Models;

namespace PrepareMail.HostedService;

public class PrepareMailWorker : MessageConsumerProcessorBase
{
    private readonly ILogger<PrepareMailWorker> _logger;
    private readonly IMessageParser _messageParser;
    private readonly IHttpService _httpService;
    private readonly IMessagePublisher _messagePublisher;
    private readonly Dictionary<EventMessageType, Func<IEnumerable<DeserializedQueueMessage<IEmailMessageBase>>, Task>> _messageTypeHandlers;
    private ConcurrentHashSet<string> _handledQueueMessages;
    public PrepareMailWorker(ILogger<PrepareMailWorker> logger, 
        IMessageParser messageParser, 
        IMessageConsumer messageConsumer,
        IHttpService httpService,
        IMessagePublisher messagePublisher) 
        : base(logger, messageConsumer)
    {
        _logger = logger;
        _messageParser = messageParser;
        _httpService = httpService;
        _messagePublisher = messagePublisher;
        _messageTypeHandlers = new Dictionary<EventMessageType, Func<IEnumerable<DeserializedQueueMessage<IEmailMessageBase>>, Task>>
        {
            { EventMessageType.PrepareMassEmailMessage, HandlePrepareMassEmailMessageAsync }
        };
    }

    protected override async Task<IEnumerable<string>> HandleMessagesAsync(ImmutableList<QueueMessage> qMessages)
    {
        try
        {
            _handledQueueMessages = new ConcurrentHashSet<string>();

            var message = ParseReceivedMessages(qMessages);

            var groupedMessagesByType = message
                .GroupBy(m => m.EventType)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );

            await Parallel.ForEachAsync(groupedMessagesByType.Keys, async (messageType, token) =>
            {
                if (_messageTypeHandlers.TryGetValue(messageType, out var typedEventMessageHandler))
                {
                    await typedEventMessageHandler.Invoke(groupedMessagesByType[messageType]);
                }
                else
                {
                    _logger.LogError($"No handler implemented for message type {messageType}");
                }
            });

            return _handledQueueMessages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return new List<string>();
        }
    }

    private async Task HandlePrepareMassEmailMessageAsync(IEnumerable<DeserializedQueueMessage<IEmailMessageBase>> messages)
    {      
        await Parallel.ForEachAsync(messages, async (message, token) =>
        {
            try 
            {
                var prepareEmailMessage = (PrepareMassEmailMessage)message.Message;

                //get XML from storage {prepareEmailMessage.DocumentPath}
                var xml = ""; 
            
                await PrepareMailForSend(xml);
                
                _handledQueueMessages.Add(message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }   
        });
    }

    private async Task PrepareMailForSend(string xml)
    {
        var clients = XmlService.DeserializeClients(xml);

        if(clients == null || clients.ClientList == null || clients.ClientList.Count == 0)
        {
            return;
        }

        //var messages = new ConcurrentBag<SendEmailMessage>();
        
        await Parallel.ForEachAsync(clients.ClientList, async (client, token) =>
        {
            var template = await GetTemplate(client.Template.Id);
            
            var filledTemplate = PopulateTemplate(template.HtmlContent, client.Template.MarketingData);

            var sendEmailMessage = new SendEmailMessage{
                ClientId = int.Parse(client.Id), //try parse but i dont time
                EmailRecipients = template.EmailRecipients,
                Subject = template.Subject,
                HtmlMessage = filledTemplate
            };

            //messages.Add(sendEmailMessage);
            await _messagePublisher.PublishMessageAsync(sendEmailMessage);
        });

        //await _messagePublisher.PublishMessagesAsync(messages);
    }

    //Assuming that HTML template contains placeholders that correspond to the keys in the JSON object
    //and there aren't nested nodes
    public string PopulateTemplate(string template, string jsonData)
    {
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);
        
        foreach (var kvp in data)
        {
            template = template.Replace($"{{{{ {kvp.Key} }}}}", kvp.Value);
        }
        
        return template;
    }

    private async Task<HtmlTemplate> GetTemplate(string templateId)
    {
        var template = await _httpService.GetAsync<HtmlTemplate>($"Template/{templateId}");
        return template;
    }

    private IEnumerable<DeserializedQueueMessage<IEmailMessageBase>> ParseReceivedMessages(ImmutableList<QueueMessage> qMessages)
    {
        var paredMessageList = new List<DeserializedQueueMessage<IEmailMessageBase>>();
        try
        {
            qMessages.ForEach(message => {
                var deserializedMessage = _messageParser.Parse<IEmailMessageBase>(message);
                paredMessageList.Add(deserializedMessage);
            });

            return paredMessageList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing message");
            return Enumerable.Empty<DeserializedQueueMessage<IEmailMessageBase>>();
        }
    }
}
