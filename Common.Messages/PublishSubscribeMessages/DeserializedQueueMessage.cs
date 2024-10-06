using System;
using Common.Messages.PublishSubscribeMessages.Interfaces;

namespace Common.Messages.PublishSubscribeMessages;

public class DeserializedQueueMessage<TMessage> where TMessage : IEventMessage
{
    public TMessage Message { get; set; }
    public EventMessageType EventType { get; set; }
    public string MessageId { get; set; }
}
