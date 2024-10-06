using System;

namespace Common.MessageBroker.Publisher.Interfaces;

public interface IMessagePublisherFactory
{
    IMessagePublisher Create();
}
