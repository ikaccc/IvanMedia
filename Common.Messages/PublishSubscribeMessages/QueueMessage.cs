using System;

namespace Common.Messages.PublishSubscribeMessages;

public class QueueMessage
{
    public string MessageBody { get; set; }
    public string MessageId { get; set; }
    public string MessageType { get; set; }

}
