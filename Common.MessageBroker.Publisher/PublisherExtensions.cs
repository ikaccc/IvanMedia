using Common.Azure.QueueService;
using Common.MessageBroker.Publisher.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.MessageBroker.Publisher;

public static class PublisherExtensions
{
    public static void ConfigurePublisherService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessagePublisherFactory, MessagePublisherFactory>(sp =>
        {
            var azServiceBusFactory = sp.GetRequiredService<IAzServiceBusFactory>();
            // var config = sp.GetRequiredService<IOptions<AzServiceBusConfig>>().Value;
            Func<ILogger<MessagePublisher>> getLoggerFunction = sp.GetRequiredService<ILogger<MessagePublisher>>;

            return new MessagePublisherFactory(azServiceBusFactory, getLoggerFunction);
        });

        services.AddSingleton<IMessagePublisher, MessagePublisher>(sp =>
        {
            var msgPubFactory = sp.GetRequiredService<IMessagePublisherFactory>();
            return (MessagePublisher)msgPubFactory.Create();
        });
    }
}
