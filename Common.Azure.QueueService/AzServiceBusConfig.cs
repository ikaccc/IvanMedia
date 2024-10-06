namespace Common.Azure.QueueService;

public class AzServiceBusConfig : IAzServiceBusConfig
{
    public static string SectionName = "AzureServiceBus";
    public string ConnectionString { get; set; }
    public string ReceiverQueueName { get; set; }
    public string SenderQueueName { get; set; }
    public int PrefetchCount { get; set; }
}
