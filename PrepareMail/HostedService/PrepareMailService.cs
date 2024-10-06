using System;
using Common.MessageBroker.Consumer.Interfaces;

namespace PrepareMail.HostedService;

public class PrepareMailService : BackgroundService
{
    private readonly ILogger<PrepareMailService> _logger;
    private readonly IMessageConsumerProcessor _worker;

    public PrepareMailService(ILogger<PrepareMailService> logger, IMessageConsumerProcessor worker)
    {
        _logger = logger;
        _worker = worker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Prepare Mail Service processor worker started..");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _worker.RunAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Prepare Mail Service");
        }

        _logger.LogInformation("Prepare Mail Service processor worker shutdown.");
    }
}
