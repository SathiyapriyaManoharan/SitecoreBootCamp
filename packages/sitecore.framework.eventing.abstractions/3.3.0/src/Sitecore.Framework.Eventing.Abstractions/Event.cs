using System;
using System.Threading.Tasks;

namespace Sitecore.Framework.Eventing
{
    public class Event<T> : IObservable<IMessage<T>>
    {
        private readonly IEventRegistry _registry;

        public Event(IEventRegistry registry, string name)
        {
            _registry = registry;
            Name = name;
        }

        public string Name { get; }

        public virtual Task Send(T value, string senderId = null) => _registry.Send(Name, value, senderId);

        public virtual IDisposable Subscribe(IObserver<IMessage<T>> observer) => _registry.Get<T>(Name).Subscribe(observer);
    }
}
