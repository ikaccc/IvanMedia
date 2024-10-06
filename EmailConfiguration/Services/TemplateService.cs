using System;
using EmailConfiguration.Models;
using EmailConfiguration.Repositories.Interfaces;
using EmailConfiguration.Services.Interfaces;


namespace EmailConfiguration.Services;

public class TemplateService : ITemplateService
{
    private readonly ITemplateRepository _templateRepository;
    private readonly ILogger<TemplateService> _logger;

    public TemplateService(ITemplateRepository templateRepository, ILogger<TemplateService> logger)
    {
        _templateRepository = templateRepository;
        _logger = logger;
    }

    public async Task<EmailTemplate> GetTemplateByIdAsync(int id)
    {
        try
        {
            return await _templateRepository.GetTemplateByIdAsync(id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get template");
            throw;
        }
    }

    public async Task<IEnumerable<EmailTemplate>> GetAllTemplatesAsync()
    {
        try
        {
            return await _templateRepository.GetAllTemplatesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get templates");
            throw;
        }
    }
    public async Task<int> CreateTemplateAsync(EmailTemplate template)
    {
        try
        {
            return await _templateRepository.CreateTemplateAsync(template).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create template");
            throw;
        }
    }

    public async Task<EmailTemplate> UpdateTemplateAsync(EmailTemplate template)
    {
        try
        {
            await _templateRepository.UpdateTemplateAsync(template).ConfigureAwait(false);
            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update template");
            throw;
        }
    }

    public async Task<bool> DeleteTemplateAsync(int id)
    {
        try
        {
            await _templateRepository.DeleteTemplateAsync(id).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete template");
            throw;
        }
    }
}
