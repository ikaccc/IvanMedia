using System;

namespace Common.MessageBroker.Consumer.Interfaces;

public interface IMessageConsumerFactory
{
    IMessageConsumer Create();
}
