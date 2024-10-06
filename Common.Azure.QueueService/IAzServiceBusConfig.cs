using System;

namespace Common.Azure.QueueService;

public interface IAzServiceBusConfig
{
    string ConnectionString { get; }
    string ReceiverQueueName { get; }
    string SenderQueueName { get; }
    int PrefetchCount { get; }
}
