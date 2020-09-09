using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Eventing
{
    public class EventRegistry : IEventRegistry
    {
        private bool _disposed = false;
        protected readonly ConcurrentDictionary<string, EventRegistration> _registrations = new ConcurrentDictionary<string, EventRegistration>();

        // We use a ConcurrentDictionary as ConcurrentBag allows duplicate entries of the same instance.
        // The cost of an extra byte is minimal, yet we keep the concurrency and (almost) the same performance.
        protected readonly ConcurrentDictionary<IDistributor, byte> _distributors = new ConcurrentDictionary<IDistributor, byte>();

        public EventRegistry(IEnumerable<IDistributor> distributors)
        {
            foreach (var distributor in distributors)
            {
                RegisterDistributor(distributor);
            }
        }

        public virtual IObservable<IMessage<T>> Get<T>(string eventName)
        {
            return GetRegistration(eventName)
                .GetConnection<T>();
        }

        public virtual async Task Send<T>(string eventName, T value, string senderId = null)
        {
            try
            {
                // wait for publish to complete
                await Publish(eventName, value, senderId);
            }
            catch (AggregateException aex)
            {
                throw new SendEventException(aex.Flatten().InnerExceptions.ToArray());
            }
        }
        
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual bool RegisterDistributor(IDistributor distributor)
        {
            if (distributor == null) throw new ArgumentNullException(nameof(distributor));

            return _distributors.TryAdd(distributor, new byte());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var registration in _registrations.Values)
                    {
                        registration.Dispose();
                    }
                }
            }
            _disposed = true;
        }

        protected virtual EventRegistration GetRegistration(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentOutOfRangeException(nameof(id));

            return _registrations.GetOrAdd(id, key => new EventRegistration());
        }

        protected virtual async Task Publish<T>(string eventName, T value, string senderId)
        {
            Condition.Requires(eventName, nameof(eventName))
                .IsNotNullOrWhiteSpace()
                .Contains(":");

            var message = new Message<T> { EventName = eventName, SenderId = senderId, Value = value };

            List<Exception> exceptions = new List<Exception>();

            Func<Exception, bool> exHandler = ex =>
            {
                exceptions.Add(ex);
                return true;
            };

            GetRegistration(eventName).Publish(message, exHandler);

            // push to parent events
            while (eventName.Contains(":"))
            {
                eventName = eventName.Substring(0, eventName.LastIndexOf(":", StringComparison.Ordinal));
                if (!string.IsNullOrWhiteSpace(eventName))
                {
                    try
                    {
                        GetRegistration(eventName).Publish(message, exHandler);
                    }
                    catch (Exception ex)
                    {
                        var sendEx = new SendEventException($"Error publishing to event stream {eventName}", ex);
                        exceptions.Add(sendEx);
                    }
                }
            }

            // push to distributors            
            List<Task> tasks = new List<Task>();

            // gather tasks
            foreach (var distributor in _distributors.Keys)
            {
                try
                {
                    tasks.Add(distributor.Distribute(message));
                }
                catch (Exception ex)
                {
                    var sendEx = new SendEventException($"Error publishing to distributor {distributor.GetType().Name}", ex);
                    exceptions.Add(sendEx);
                }
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (AggregateException aex)
            {
                foreach (var innerException in aex.Flatten().InnerExceptions)
                {
                    var sendEx = new SendEventException($"Error while publishing to distributors", innerException);
                    exceptions.Add(sendEx);
                }
            }
            catch (Exception ex)
            {
                var sendEx = new SendEventException($"Error while publishing to distributors", ex);
                exceptions.Add(sendEx);
            }
            finally
            {
                if (exceptions.Any())
                    throw new AggregateException("Publish Exceptions", exceptions);
            }
        }

        protected class EventRegistration : IDisposable
        {
            private bool _disposed = false;

            private readonly ExceptionHandlingSubject<IMessage> _subject = new ExceptionHandlingSubject<IMessage>();
            private readonly ConcurrentDictionary<Type, IObservable<IMessage>> _connections = new ConcurrentDictionary<Type, IObservable<IMessage>>();
            
            public IObservable<IMessage<T>> GetConnection<T>()
            {
                return _connections.GetOrAdd(typeof(T), key => CreateConnection<T>()) as IObservable<IMessage<T>>;
            }

            public void Publish(IMessage message, Func<Exception, bool> exceptionHandler)
            {
                _subject.OnNext(message, exceptionHandler);
            }

            private IObservable<IMessage<T>> CreateConnection<T>()
            {
                var connectable = _subject
                    .Select(m => m.ValueAs<T>())
                    .Where(m => m != null);
                                
                return connectable.AsObservable();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _subject.OnCompleted();
                        _subject.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}