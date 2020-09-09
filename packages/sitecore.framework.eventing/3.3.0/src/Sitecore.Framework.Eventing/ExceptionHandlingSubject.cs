using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace Sitecore.Framework.Eventing
{
    // Ripped directly from https://github.com/Reactive-Extensions/Rx.NET/blob/master/Rx.NET/Source/System.Reactive.Linq/Reactive/Subjects/Subject.cs

    /// <summary>
    /// Standard subject exposing a method to handle the subscription errors if thrown
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExceptionHandlingSubject<T> : ISubject<T>, IDisposable
    {
        private bool isDisposed;
        private bool isStopped;
        // TODO: reintroduce immutablity? All list access is already within lock(gate){}
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();
        object gate = new object();
        Exception exception;

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            var os = default(IObserver<T>[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.ToArray();
                    observers.Clear();
                    isStopped = true;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnCompleted();
        }

        /// <summary>
        /// Notifies all subscribed observers with the exception.
        /// </summary>
        /// <param name="error">The exception to send to all subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            var os = default(IObserver<T>[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.ToArray();
                    observers.Clear();
                    isStopped = true;
                    exception = error;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnError(error);
        }

        /// <summary>
        /// Notifies all subscribed observers with the value.
        /// </summary>
        /// <param name="value">The value to send to all subscribed observers.</param>
        public void OnNext(T value)
        {
            OnNext(value, x => false);
        }

        /// <summary>
        /// Notifies all subscribed observers with the value.
        /// </summary>
        /// <param name="value">The value to send to all subscribed observers.</param>
        /// <param name="exceptionHandler">Handler for exceptions, return true to continue with next subscriber, false to stop processing.</param>
        public void OnNext(T value, Func<Exception, bool> exceptionHandler)
        {
            var os = default(IObserver<T>[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.ToArray();
                }
            }

            if (os == null) return;

            foreach (var o in os)
            {
                try
                {
                    o.OnNext(value);
                }
                catch (Exception ex)
                {
                    if (!exceptionHandler(ex))
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <remarks>IDisposable object that can be used to unsubscribe the observer from the subject.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    observers.Add(observer);
                    return new Subscription(this, observer);
                }
                else if (exception != null)
                {
                    observer.OnError(exception);
                    return Disposable.Empty;
                }
                else
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }
            }
        }

        void Unsubscribe(IObserver<T> observer)
        {
            lock (gate)
            {
                observers.Remove(observer);
            }
        }

        class Subscription : IDisposable
        {
            ExceptionHandlingSubject<T> subject;
            IObserver<T> observer;

            public Subscription(ExceptionHandlingSubject<T> subject, IObserver<T> observer)
            {
                this.subject = subject;
                this.observer = observer;
            }

            public void Dispose()
            {
                var o = Interlocked.Exchange(ref observer, null);
                if (o != null)
                {
                    subject.Unsubscribe(o);
                    subject = null;
                }
            }
        }

        void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public void Dispose()
        {
            lock (gate)
            {
                isDisposed = true;
                observers.Clear();
            }
        }
    }
}
