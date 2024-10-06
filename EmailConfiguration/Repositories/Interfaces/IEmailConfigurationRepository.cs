using System;
using EmailConfiguration.Models;

namespace EmailConfiguration.Repositories.Interfaces;

public interface IEmailConfigurationRepository
{
    Task<ClientEmailConfiguration> GetConfigurationByIdAsync(int id);
    Task<IEnumerable<ClientEmailConfiguration>> GetAllConfigurationsAsync();
    Task<int> CreateConfigurationAsync(ClientEmailConfiguration configuration);
    Task UpdateConfigurationAsync(ClientEmailConfiguration configuration);
    Task DeleteConfigurationAsync(int id);
    Task<ClientEmailConfiguration> GetConfigurationByClientIdAsync(int clientId);
}
