using System;
using Common.MessageBroker.Consumer.Interfaces;

namespace Listener.HostedService;

public class ListenerService : BackgroundService
{
    private readonly ILogger<ListenerService> _logger;
    private readonly IMessageConsumerProcessor _worker;

    public ListenerService(ILogger<ListenerService> logger, IMessageConsumerProcessor worker)
    {
        _logger = logger;
        _worker = worker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Sender Processor Service processor worker started..");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _worker.RunAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Email Sender Processor Service");
        }

        _logger.LogInformation("Email Sender Processor Service processor worker shutdown.");
    }
}
