using System;
namespace Common.EmailSender.Models;

public class EmailConfiguration
{
    public string DisplayName { get; set; }
    public string SmtpServer { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public bool UseSsl { get; set; }
}

