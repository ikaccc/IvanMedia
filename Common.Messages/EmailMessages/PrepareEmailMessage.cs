using System;
using Common.Messages.EmailMessages.Interfaces;

namespace Common.Messages.EmailMessages;

public class PrepareMassEmailMessage : IEmailMessage<PrepareMassEmailMessage>, IEmailMessageBase
{
    public int ClientId { get; set; }
    //system will store XML as a file on clude storage, and push massage with a link 
    public string DocumentPath { get; set; }
    public string TemplateName { get; set; }
    public DateTime UtcReceived { get; set; }
    public DateTime UtcCreated { get; set; }
    public string MessageId { get; set; }

    public string SessionId => ClientId.ToString();
}
