using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Azure.QueueService;

    public static class AzServiceBusClientFactoryExtensions
    {
        public static void ConfigureAzureServiceBus(this IServiceCollection services, IConfiguration configurationRoot)
        {
            services.AddSingleton(sp =>
            {
                var connString = sp.GetRequiredService<IOptions<AzServiceBusConfig>>().Value;
                return new ServiceBusClient(connString.ConnectionString);
            });

            services.AddSingleton<IAzServiceBusFactory, AzServiceBusFactory>(s =>
            {
                var sqsConfig = s.GetRequiredService<IOptions<AzServiceBusConfig>>().Value;
                var serviceBusClient = s.GetRequiredService<ServiceBusClient>();

                return new AzServiceBusFactory(sqsConfig, serviceBusClient);
            });
        }
    }
