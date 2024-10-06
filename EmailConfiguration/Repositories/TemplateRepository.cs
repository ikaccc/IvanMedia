using System;
using Microsoft.EntityFrameworkCore;
using EmailConfiguration.Repositories.Interfaces;
using EmailConfiguration.Data;
using EmailConfiguration.Models;

namespace EmailConfiguration.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly EmailConfigurationDbContext _context;
    private readonly ILogger<TemplateRepository> _logger;

    public TemplateRepository(EmailConfigurationDbContext context, ILogger<TemplateRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmailTemplate> GetTemplateByIdAsync(int id)
    {
        try 
        {
            return await _context.EmailTemplates.FindAsync(id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get template by id");
            throw;
        }
    }

    public async Task<IEnumerable<EmailTemplate>> GetAllTemplatesAsync()
    {
        try
        {
            return await _context.EmailTemplates.ToListAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all templates");
            throw;
        }
    }

    public async Task<int> CreateTemplateAsync(EmailTemplate template)
    {
        template.CreatedAt = DateTime.UtcNow;
        template.UpdatedAt = DateTime.UtcNow;
        try
        {
            await _context.EmailTemplates.AddAsync(template).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return template.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create template");
            throw;
        }
    }

    public async Task UpdateTemplateAsync(EmailTemplate template)
    {
        template.UpdatedAt = DateTime.UtcNow;
        try
        {
            _context.EmailTemplates.Update(template);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update template");
            throw;
        }
    }

    public async Task DeleteTemplateAsync(int id)
    {
        try
        {
            var template = await _context.EmailTemplates.FindAsync(id).ConfigureAwait(false);
            if (template != null)
            {
                _context.EmailTemplates.Remove(template);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete template");
            throw;
        }
    }
}

