using System;
using System.Text.Json.Serialization;
using Common.Messages.PublishSubscribeMessages.Interfaces;

namespace Common.Messages.EmailMessages.Interfaces;

/// <summary>
/// Usage of CRTP
/// https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern
/// JsonDerivedType attribute is used because of Polymorphic serialization
/// </summary>
[JsonDerivedType(typeof(SendEmailMessage), typeDiscriminator: nameof(SendEmailMessage))]
public interface IEmailMessageBase : IEmailMessage<IEmailMessageBase>, IEventMessage
{ }

public interface IEmailMessage<T> where T : IEmailMessage<T>
{
    int ClientId { get; set; }
    DateTime UtcReceived { get; set; }
    DateTime UtcCreated { get; set; }
    string MessageId { get; set; }
}