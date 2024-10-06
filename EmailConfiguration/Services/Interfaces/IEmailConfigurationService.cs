using System;
using EmailConfiguration.Models;

namespace EmailConfiguration.Services.Interfaces;

public interface IEmailConfigurationService
{
    Task<ClientEmailConfiguration> GetConfigurationByIdAsync(int id);
    Task<IEnumerable<ClientEmailConfiguration>> GetAllConfigurationsAsync();
    Task<int> CreateConfigurationAsync(ClientEmailConfiguration configuration);
    Task UpdateConfigurationAsync(ClientEmailConfiguration configuration);
    Task<bool> DeleteConfigurationAsync(int id);
    Task<ClientEmailConfiguration> GetConfigurationByClientIdAsync(int clientId);
}