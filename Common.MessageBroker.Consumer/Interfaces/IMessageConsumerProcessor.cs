using System;
using Common.Messages;

namespace Common.MessageBroker.Consumer.Interfaces;

public interface IMessageConsumerProcessor
{
    Task RunAsync();
}
