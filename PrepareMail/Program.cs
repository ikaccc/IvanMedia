using Common.Azure.QueueService;
using Common.HttpService;
using Common.MessageBroker.Consumer.Extensions;
using Common.MessageBroker.Consumer.Interfaces;
using Common.MessageBroker.Publisher;
using Microsoft.Extensions.Options;
using PrepareMail.Configuration;
using PrepareMail.HostedService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<EmailConfigurationApi>(builder.Configuration.GetSection(EmailConfigurationApi.SectionName));
builder.Services.Configure<AzServiceBusConfig>(builder.Configuration.GetSection(AzServiceBusConfig.SectionName));

builder.Services.AddHttpClient<IHttpService, HttpService>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<EmailConfigurationApi>>().Value;
    client.BaseAddress = new Uri(settings.Url);
});

builder.Services.AddTransient<IHttpService, HttpService>();
builder.Services.ConfigureAzureServiceBus(builder.Configuration);
builder.Services.ConfigureConsumerService(builder.Configuration);
builder.Services.ConfigurePublisherService(builder.Configuration);
builder.Services.AddSingleton<IMessageConsumerProcessor, PrepareMailWorker>();

builder.Services.AddHostedService<PrepareMailService>();

var host = builder.Build();
host.Run();
