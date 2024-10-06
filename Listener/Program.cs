using Common.Azure.QueueService;
using Common.EmailSender;
using Common.EmailSender.Iterfaces;
using Common.HttpService;
using Common.MessageBroker.Consumer.Extensions;
using Common.MessageBroker.Consumer.Interfaces;
using Listener.Configuration;
using Listener.HostedService;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<EmailConfigurationApi>(builder.Configuration.GetSection(EmailConfigurationApi.SectionName));
builder.Services.Configure<AzServiceBusConfig>(builder.Configuration.GetSection(AzServiceBusConfig.SectionName));

builder.Services.AddHttpClient<IHttpService, HttpService>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<EmailConfigurationApi>>().Value;
    client.BaseAddress = new Uri(settings.Url);
});

builder.Services.AddTransient<IHttpService, HttpService>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.ConfigureAzureServiceBus(builder.Configuration);
builder.Services.ConfigureConsumerService(builder.Configuration);
builder.Services.AddSingleton<IMessageConsumerProcessor, EmailSenderWorker>();

builder.Services.AddHostedService<ListenerService>();

var host = builder.Build();
host.Run();

