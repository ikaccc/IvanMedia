using System;
using Microsoft.EntityFrameworkCore;
using EmailConfiguration.Repositories.Interfaces;
using EmailConfiguration.Models;
using EmailConfiguration.Data;
namespace EmailConfiguration.Repositories;

public class EmailConfigurationRepository : IEmailConfigurationRepository
{
    private readonly EmailConfigurationDbContext _context;
    private readonly ILogger<EmailConfigurationRepository> _logger;

    public EmailConfigurationRepository(EmailConfigurationDbContext context, ILogger<EmailConfigurationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ClientEmailConfiguration> GetConfigurationByIdAsync(int id)
    {
        try
        {
            return await _context.EmailConfigurations.FindAsync(id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get configuration by id");
            throw;
        }
    }

    public async Task<IEnumerable<ClientEmailConfiguration>> GetAllConfigurationsAsync()
    {
        try
        {
            return await _context.EmailConfigurations.ToListAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all configurations");
            throw;
        }
    }

    public async Task<int> CreateConfigurationAsync(ClientEmailConfiguration configuration)
    {
        configuration.CreatedAt = DateTime.UtcNow;
        configuration.UpdatedAt = DateTime.UtcNow;

        try
        {
            var result = await _context.EmailConfigurations.AddAsync(configuration).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return configuration.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create configuration");
            throw;
        }
    }

    public async Task UpdateConfigurationAsync(ClientEmailConfiguration configuration)
    {
        configuration.UpdatedAt = DateTime.UtcNow;
        
        try
        {
            _context.EmailConfigurations.Update(configuration);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update configuration");
            throw;
        }
    }

    public async Task DeleteConfigurationAsync(int id)
    {
        try
        {
            var configuration = await _context.EmailConfigurations.FindAsync(id).ConfigureAwait(false);
            if (configuration != null)
            {
                _context.EmailConfigurations.Remove(configuration);
                await _context.SaveChangesAsync();
            }
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
            return await _context.EmailConfigurations.FirstOrDefaultAsync(c => c.ClientId == clientId).ConfigureAwait(false);
        }
        catch (Exception ex)    
        {
            _logger.LogError(ex, "Failed to get configuration by client id");
            throw;
        }
    }
}
