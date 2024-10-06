using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Azure.QueueService;
using Common.MessageBroker.Consumer.Interfaces;
using Microsoft.Extensions.Logging;
using Common.MessageBroker.Consumer.Utilities;

namespace Common.MessageBroker.Consumer.Extensions;

public static class ConsumerExtensions
{
    public static void ConfigureConsumerService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageConsumerFactory, MessageConsumerFactory>(sp =>
        {
            var azServiceBusFactory = sp.GetRequiredService<IAzServiceBusFactory>();
            //var config = sp.GetRequiredService<IOptions<AzServiceBusConfig>>().Value;
            Func<ILogger<MessageConsumer>> getLoggerFunc = sp.GetRequiredService<ILogger<MessageConsumer>>;
            return new MessageConsumerFactory(azServiceBusFactory, getLoggerFunc);
        });

        services.AddSingleton<IMessageParser, MessageParser>();

        services.AddSingleton<IMessageConsumer, MessageConsumer>(sp =>
        {
            var messageConsumerFactory = sp.GetRequiredService<IMessageConsumerFactory>();
            return (MessageConsumer) messageConsumerFactory.Create();
        });
    }
}
