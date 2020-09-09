using System;
using System.Threading.Tasks;

namespace Sitecore.Framework.Eventing
{
    public interface IEventRegistry
    {
        Task Send<T>(string eventName, T value, string senderId = null);
        
        IObservable<IMessage<T>> Get<T>(string eventName);        
    }
}