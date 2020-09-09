namespace Sitecore.Framework.Eventing
{
    public static class MessageExtensions
    {
        public static IMessage<T> ValueAs<T>(this IMessage message)
        {
            var type = message as IMessage<T>;
            if (type != null) return type;

            if (message.Value is T)
            {
                return new Message<T>
                {
                    EventName = message.EventName,
                    SenderId = message.SenderId,
                    Value = (T)message.Value,
                    Id = message.Id,
                    TimeStamp = message.TimeStamp
                };
            }

            return null;
        }
    }
}
