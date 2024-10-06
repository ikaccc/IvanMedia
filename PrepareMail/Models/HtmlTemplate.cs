using System;

namespace PrepareMail.Models;

public class HtmlTemplate
{
    public string EmailRecipients { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
}
