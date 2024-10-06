using System;
using Azure.Messaging.ServiceBus;


namespace Common.Azure.QueueService;

public interface IAzServiceBusFactory
{
    (ServiceBusClient client, ServiceBusReceiver receiver) CreateClientAndReceiver();
    (ServiceBusClient client, ServiceBusSender sender) CreateClientAndSender();
    (ServiceBusClient client, ServiceBusSender sender, ServiceBusReceiver receiver) CreateClientSenderAndReceiver();
}
