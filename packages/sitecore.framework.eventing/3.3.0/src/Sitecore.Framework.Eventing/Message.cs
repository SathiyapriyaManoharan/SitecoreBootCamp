using System;

namespace Sitecore.Framework.Eventing
{
    public class Message : IMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public string EventName { get; set; }

        public string SenderId { get; set; }

        public object Value { get; set; }
    }

    public class Message<T> : Message, IMessage<T>
    {
        public virtual new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }
    }
}