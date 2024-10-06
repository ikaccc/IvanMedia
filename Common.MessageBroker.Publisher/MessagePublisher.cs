using System;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Common.Azure.QueueService;
using Common.MessageBroker.Publisher.Interfaces;
using Common.Messages.PublishSubscribeMessages.Interfaces;
using Microsoft.Extensions.Logging;

namespace Common.MessageBroker.Publisher;

public class MessagePublisher : IMessagePublisher
{
    private readonly ILogger<MessagePublisher> _logger;
    private readonly ServiceBusSender _sender;

    public MessagePublisher(IAzServiceBusFactory azServiceBusClientReceiverFactory,
    ILogger<MessagePublisher> logger)
    {
        _logger = logger;
        var (_, sender) = azServiceBusClientReceiverFactory.CreateClientAndSender();
        _sender = sender;
    }

    public async Task PublishMessageAsync<TMessage>(TMessage message) where TMessage : IEventMessage
    {
        try 
        {
            string jsonMessage = JsonSerializer.Serialize(message);

            var serviceBusMessage = new ServiceBusMessage(jsonMessage)
            {
                Subject = typeof(TMessage).Name,
            };

            await _sender.SendMessageAsync(serviceBusMessage).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message");
            throw;
        }   
    }


    public async Task PublishMessagesAsync<TMessage>(IEnumerable<TMessage> messages) where TMessage : IEventMessage
    {
        var listOfMessages = new List<ServiceBusMessage>();

        try 
        {
            foreach (var message in messages)   
            {
                string jsonMessage = JsonSerializer.Serialize(message);

                var serviceBusMessage = new ServiceBusMessage(jsonMessage)
                {
                    Subject = typeof(TMessage).Name,
                };
                listOfMessages.Add(serviceBusMessage);
            }
        
            await _sender.SendMessagesAsync(listOfMessages).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing messages");
            throw;
        }
    }
}
