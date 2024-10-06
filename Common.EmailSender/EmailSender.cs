using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Common.EmailSender.Iterfaces;
using Common.EmailSender.Models;
using Microsoft.Extensions.Logging;

namespace Common.EmailSender;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(int clientId, string emailRecipients, string subject, string htmlMessage, EmailConfiguration emailConfiguration)
    {
        if (emailConfiguration == null)
        {
            return;
        }

        try
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(emailConfiguration.SenderEmail, emailConfiguration.DisplayName);
                ConfigureRecipients(message, emailRecipients);

                message.Subject = subject;
                message.Body = htmlMessage;
                message.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);
                    smtpClient.EnableSsl = true;

                    _logger.LogInformation("Sending email to {RecipientEmail}", emailRecipients);

                    await smtpClient.SendMailAsync(message).ConfigureAwait(false);

                    _logger.LogInformation("Email sent successfully to {RecipientEmail}", emailRecipients);
                }
            }
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SMTP error occurred while sending email to {RecipientEmail}", emailRecipients);
            // TODO: Send to Sentry or database
            // create a new log entry in the database with the error message clientId, emailRecipients, subject, htmlMessage 
            // for future retry policy implementation
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending email to {RecipientEmail}", emailRecipients);
            // TODO: Send to Sentry or database
        }
    }

    private bool IsValidEmail(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        return regex.Match(email).Success;
    }

    private void ConfigureRecipients(MailMessage message, string emailRecipients)
    {
        var recipients = emailRecipients.Split(';');

        foreach (var recipient in recipients)
        {
            if (!IsValidEmail(recipient))
            {
                _logger.LogWarning("Invalid email address: {RecipientEmail}", recipient);
            }
            else
            {
                message.To.Add(new MailAddress(recipient));
            }
        }
    }
}
