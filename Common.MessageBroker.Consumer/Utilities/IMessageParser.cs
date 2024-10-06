using System;
using Common.Messages.PublishSubscribeMessages;
using Common.Messages.PublishSubscribeMessages.Interfaces;

namespace Common.MessageBroker.Consumer.Utilities;

public interface IMessageParser
{
    DeserializedQueueMessage<TMessage> Parse<TMessage>(QueueMessage queueMessage)
        where TMessage : IEventMessage;
}
