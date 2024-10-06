using System;
using EmailConfiguration.Models;
using EmailConfiguration.Repositories.Interfaces;
using EmailConfiguration.Services.Interfaces;

namespace EmailConfiguration.Services;

public class EmailConfigurationService : IEmailConfigurationService
{
    private readonly IEmailConfigurationRepository _configurationRepository;
    private readonly ILogger<EmailConfigurationService> _logger;

    public EmailConfigurationService(IEmailConfigurationRepository configurationRepository, ILogger<EmailConfigurationService> logger)
    {
        _configurationRepository = configurationRepository;
        _logger = logger;
    }

    public async Task<ClientEmailConfiguration> GetConfigurationByIdAsync(int id)
    {
        try 
        {
            var configuration = await _configurationRepository.GetConfigurationByIdAsync(id).ConfigureAwait(false);
            return configuration;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get configuration");
            throw;
        }
    }

    public async Task<IEnumerable<ClientEmailConfiguration>> GetAllConfigurationsAsync()
    {
        try 
        {
            var configurations = await _configurationRepository.GetAllConfigurationsAsync().ConfigureAwait(false);
            return configurations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get configurations");
            throw;
        }
    }

    public async Task<int> CreateConfigurationAsync(ClientEmailConfiguration configuration)
    {
        try 
        {
            return await _configurationRepository.CreateConfigurationAsync(configuration).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create configuration");
            throw;
        }
    }

    public async Task UpdateConfigurationAsync(ClientEmailConfiguration configuration)
    {
        try 
        {
            await _configurationRepository.UpdateConfigurationAsync(configuration).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update configuration");
            throw;
        }
    }

    public async Task<bool> DeleteConfigurationAsync(int id)
    {
        try 
        {
            await _configurationRepository.DeleteConfigurationAsync(id).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete configuration");
            throw;
        }
    }

    public async Task<ClientEmailConfiguration> GetConfigurationByClientIdAsync(int clientId)
    {
        try     
        {
            var configuration = await _configurationRepository.GetConfigurationByClientIdAsync(clientId).ConfigureAwait(false);
            return configuration;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get configuration");
            throw;
        }
    }
}
