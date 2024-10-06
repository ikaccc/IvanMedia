using System.Collections.Concurrent;
using System.Collections.Immutable;
using Common.EmailSender.Iterfaces;
using Common.EmailSender.Models;
using Common.HttpService;
using Common.MessageBroker.Consumer;
using Common.MessageBroker.Consumer.Interfaces;
using Common.MessageBroker.Consumer.Utilities;
using Common.Messages.EmailMessages;
using Common.Messages.EmailMessages.Interfaces;
using Common.Messages.PublishSubscribeMessages;

namespace Listener.HostedService;

public class EmailSenderWorker : MessageConsumerProcessorBase
{
    private readonly ILogger<EmailSenderWorker> _logger;
    private readonly IMessageParser _messageParser;
    private readonly IEmailSender _emailSender;
    private readonly IHttpService _httpService;

    private readonly Dictionary<
    EventMessageType,
    Func<IEnumerable<DeserializedQueueMessage<IEmailMessageBase>>, Task>> _messageTypeHandlers;
    private ConcurrentHashSet<string> _handledQueueMessages;
    public EmailSenderWorker(ILogger<EmailSenderWorker> logger, 
        IMessageParser messageParser, 
        IEmailSender emailSender,
        IMessageConsumer messageConsumer,
        IHttpService httpService) 
        : base(logger, messageConsumer)
    {
        _logger = logger;
        _messageParser = messageParser;
        _emailSender = emailSender;
        _httpService = httpService;
        _messageTypeHandlers = new Dictionary<EventMessageType, Func<IEnumerable<DeserializedQueueMessage<IEmailMessageBase>>, Task>>
        {
            { EventMessageType.SendEmailMessage, HandleSendEmailMessageAsync }
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
                    await typedEventMessageHandler.Invoke(groupedMessagesByType[messageType]).ConfigureAwait(false);
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

    private async Task HandleSendEmailMessageAsync(IEnumerable<DeserializedQueueMessage<IEmailMessageBase>> messages)
    {      

        var tasks = new ConcurrentBag<Task>();
        await Parallel.ForEachAsync(messages, async (message, token) =>
        {
            try 
            {
                var sendEmailMessage = (SendEmailMessage)message.Message;
                var mailConfiguration = await _httpService.GetAsync<EmailConfiguration>($"EmailConfiguration/GetConfigurationByClientId/{sendEmailMessage.ClientId}").ConfigureAwait(false);
                tasks.Add(_emailSender.SendEmailAsync(sendEmailMessage.ClientId, sendEmailMessage.EmailRecipients, sendEmailMessage.Subject, sendEmailMessage.HtmlMessage, mailConfiguration));
                //await _emailSender.SendEmailAsync(sendEmailMessage.ClientId, sendEmailMessage.EmailRecipients, sendEmailMessage.Subject, sendEmailMessage.HtmlMessage, mailConfiguration
                _handledQueueMessages.Add(message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
            }   
        });

        await Task.WhenAll(tasks);
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

