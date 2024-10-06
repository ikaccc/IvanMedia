using System;
using EmailConfiguration.Models;

namespace EmailConfiguration.Repositories.Interfaces;

public interface ITemplateRepository
{
    Task<EmailTemplate> GetTemplateByIdAsync(int id);
    Task<IEnumerable<EmailTemplate>> GetAllTemplatesAsync();
    Task<int> CreateTemplateAsync(EmailTemplate template);
    Task UpdateTemplateAsync(EmailTemplate template);
    Task DeleteTemplateAsync(int id);
}

