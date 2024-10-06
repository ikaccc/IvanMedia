using System;
using Common.EmailSender.Models;

namespace Common.EmailSender.Iterfaces;

public interface IEmailSender
{
    Task SendEmailAsync(int clientId, string emailRecipients, string subject, string htmlMessage, EmailConfiguration emailConfiguration);
}

