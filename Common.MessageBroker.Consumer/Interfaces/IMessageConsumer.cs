using System;
using Azure.Messaging.ServiceBus;
using Common.Messages.PublishSubscribeMessages;

namespace Common.MessageBroker.Consumer.Interfaces;

public interface IMessageConsumer
{
    Task<(IEnumerable<QueueMessage> queueMessages, IReadOnlyList<ServiceBusReceivedMessage> originalMessages)> ReceiveMessagesAsync();
    Task DeleteMessagesAsync(IEnumerable<ServiceBusReceivedMessage> messages);
}
