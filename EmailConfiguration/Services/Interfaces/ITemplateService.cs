using System;
using EmailConfiguration.Models;

namespace EmailConfiguration.Services.Interfaces;

public interface ITemplateService
{
    Task<EmailTemplate> GetTemplateByIdAsync(int id);
    Task<IEnumerable<EmailTemplate>> GetAllTemplatesAsync();
    Task<int> CreateTemplateAsync(EmailTemplate template);
    Task<EmailTemplate> UpdateTemplateAsync(EmailTemplate template);
    Task<bool> DeleteTemplateAsync(int id);
}
