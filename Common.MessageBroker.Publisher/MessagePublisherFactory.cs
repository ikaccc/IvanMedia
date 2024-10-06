using System;
using Common.Azure.QueueService;
using Common.MessageBroker.Publisher.Interfaces;
using Microsoft.Extensions.Logging;

namespace Common.MessageBroker.Publisher;

public class MessagePublisherFactory : IMessagePublisherFactory
{
    private readonly IAzServiceBusFactory _azServiceBusClientReceiverFactory;
    private readonly Func<ILogger<MessagePublisher>> _logger;

    public MessagePublisherFactory(IAzServiceBusFactory azServiceBusClientReceiverFactory, Func<ILogger<MessagePublisher>> getLogger)
    {
        _azServiceBusClientReceiverFactory = azServiceBusClientReceiverFactory;
        _logger = getLogger;
    }   

    public IMessagePublisher Create()
    {
        return new MessagePublisher(_azServiceBusClientReceiverFactory, _logger());
    }
}
