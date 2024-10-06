using System;

namespace EmailConfiguration.Models;

public class ClientEmailConfiguration
{
    public int Id { get; set; }
    public int ClientId { get; set; } 
    public string DisplayName { get; set; }
    public string SmtpServer { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public bool UseSsl { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}