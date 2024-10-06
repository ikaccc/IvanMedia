using System;

namespace Common.Messages.PublishSubscribeMessages;

    [AttributeUsage(AttributeTargets.Class)]
    public class MessageBindingAttribute : Attribute
    {
        public EventMessageType MessageType { get; set; }

        public MessageBindingAttribute(EventMessageType messageType)
        {
            MessageType = messageType;
        }
    }
