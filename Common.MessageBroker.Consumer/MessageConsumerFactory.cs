using System;
using Common.Azure.QueueService;
using Common.MessageBroker.Consumer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Common.MessageBroker.Consumer;

public class MessageConsumerFactory : IMessageConsumerFactory
{
    private readonly IAzServiceBusFactory _azServiceBusClientReceiverFactory;
    private readonly Func<ILogger<MessageConsumer>> _logger;

    public MessageConsumerFactory(IAzServiceBusFactory azServiceBusClientReceiverFactory, Func<ILogger<MessageConsumer>> getLogger)
    {
        _azServiceBusClientReceiverFactory = azServiceBusClientReceiverFactory;
        _logger = getLogger;
    }   

    public IMessageConsumer Create()
    {
        return new MessageConsumer(_azServiceBusClientReceiverFactory, _logger());
    }
}
