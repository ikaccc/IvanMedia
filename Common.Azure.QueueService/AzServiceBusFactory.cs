using System;
using Azure.Messaging.ServiceBus;
namespace Common.Azure.QueueService;

public class AzServiceBusFactory : IAzServiceBusFactory
{
    private readonly IAzServiceBusConfig _config;
    private readonly ServiceBusClient _azBusClient;

    public AzServiceBusFactory(IAzServiceBusConfig config, ServiceBusClient azBusClient)
    {
        _config = config;
        _azBusClient = azBusClient;
    }

    public (ServiceBusClient client, ServiceBusReceiver receiver) CreateClientAndReceiver()
    {
        var azReceiver = _azBusClient.CreateReceiver(_config.ReceiverQueueName);
        return (_azBusClient, azReceiver);
    }

    public (ServiceBusClient client, ServiceBusSender sender) CreateClientAndSender()
    {
        var azSender = _azBusClient.CreateSender(_config.SenderQueueName);
        return (_azBusClient, azSender);
    }

    public (ServiceBusClient client, ServiceBusSender sender, ServiceBusReceiver receiver) CreateClientSenderAndReceiver()
    {
        var azSender = _azBusClient.CreateSender(_config.SenderQueueName);
        var azReceiver = _azBusClient.CreateReceiver(_config.ReceiverQueueName);
        return (_azBusClient, azSender, azReceiver);
    }
}
