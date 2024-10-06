using System;
using Azure.Messaging.ServiceBus;
using Common.Azure.QueueService;
using Common.MessageBroker.Consumer.Interfaces;
using Common.Messages.PublishSubscribeMessages;
using Microsoft.Extensions.Logging;

namespace Common.MessageBroker.Consumer;

public class MessageConsumer : IMessageConsumer
{
    private readonly int _maxQueueMessagesPerCall = 10;
    private readonly ServiceBusReceiver _receiver;
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(IAzServiceBusFactory azServiceBusClientReceiverFactory,
        ILogger<MessageConsumer> logger)
    {
        var (_, receiver) = azServiceBusClientReceiverFactory.CreateClientAndReceiver();
        _receiver = receiver;
        _logger = logger;
    }   

    public async Task<(IEnumerable<QueueMessage> queueMessages, IReadOnlyList<ServiceBusReceivedMessage> originalMessages)> ReceiveMessagesAsync()
    {
        var messages = await _receiver.ReceiveMessagesAsync(maxMessages: _maxQueueMessagesPerCall, maxWaitTime: TimeSpan.FromSeconds(5)).ConfigureAwait(false);

        return (GetQueueMessagesFromServiceBusReceivedMessage(messages), messages);
    }

    public async Task DeleteMessagesAsync(IEnumerable<ServiceBusReceivedMessage> messages)
    {
        if (messages == null || (messages != null && messages.Count() == 0))
        {
            return;
        }

        foreach (var message in messages)
        {
            await _receiver.CompleteMessageAsync(message).ConfigureAwait(false);
        }
    }

    private IEnumerable<QueueMessage> GetQueueMessagesFromServiceBusReceivedMessage(IEnumerable<ServiceBusReceivedMessage> messageResponseMessages) =>
        messageResponseMessages.Select(
            message => new QueueMessage
            {
                MessageBody = System.Text.Encoding.UTF8.GetString(message.Body),
                MessageId = message.MessageId,
                MessageType = message.Subject
            });
}
