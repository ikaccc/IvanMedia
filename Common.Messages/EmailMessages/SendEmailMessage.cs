using System;
using Common.Messages.EmailMessages.Interfaces;

namespace Common.Messages.EmailMessages;

public class SendEmailMessage : IEmailMessage<SendEmailMessage>, IEmailMessageBase
{
    public int ClientId { get; set; }
    public string EmailRecipients { get; set; }
    public string Subject { get; set; }
    public string HtmlMessage { get; set; }
    public DateTime UtcReceived { get; set; }
    public DateTime UtcCreated { get; set; }
    public string MessageId { get; set; }

    public string SessionId => ClientId.ToString();
}
