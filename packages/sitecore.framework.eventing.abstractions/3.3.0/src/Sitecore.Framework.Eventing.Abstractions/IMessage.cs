using System;

namespace Sitecore.Framework.Eventing
{
    public interface IMessage
    {
        string Id { get; }

        DateTime TimeStamp { get; }

        string EventName { get; }

        string SenderId { get; }

        object Value { get; }        
    }

    public interface IMessage<T> : IMessage
    {
        new T Value { get; }
    }
}