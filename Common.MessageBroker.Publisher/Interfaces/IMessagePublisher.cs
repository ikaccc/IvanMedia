using System;
using Common.Messages.EmailMessages.Interfaces;
using Common.Messages.PublishSubscribeMessages.Interfaces;

namespace Common.MessageBroker.Publisher.Interfaces;

public interface IMessagePublisher
{
    Task PublishMessageAsync<TMessage>(TMessage message) where TMessage : IEventMessage;
    Task PublishMessagesAsync<TMessage>(IEnumerable<TMessage> messages) where TMessage : IEventMessage;
}
