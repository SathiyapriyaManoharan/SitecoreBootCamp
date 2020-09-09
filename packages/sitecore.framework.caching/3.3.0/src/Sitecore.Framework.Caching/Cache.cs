using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sitecore.Framework.Conditions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

namespace Sitecore.Framework.Caching
{
    public class Cache : ICache, IPrioritizableCache, ICacheStatistics
    {
        private readonly ICacheStore _store;
        private CacheEntryPriority _defaultPriority;
        private int _hits;
        private int _misses;
        private List<string> _keys = new List<string>();
        private object _keysLock = new object();
        

        public string StoreName { get; }

        public string Name { get; }

        public Cache(ICacheStore store, string name)
        {
            Condition.Requires(store, nameof(store)).IsNotNull();
            Condition.Requires(name, nameof(name)).IsNotNull();

            _store = store;
            Name = name;
            StoreName = store.Name;
        }

        public virtual Task Set(string key, ICachable value, CacheEntryOptions entryOptions = null)
        {
            Condition.Requires(key, nameof(key)).IsNotNull();
            Condition.Requires(value, nameof(value)).IsNotNull();

            if (entryOptions == null)
                entryOptions = new CacheEntryOptions();

            var prefixedKey = CreateKey(key);
            entryOptions.Priority = entryOptions.Priority ?? _defaultPriority;

            lock(_keysLock)
            {
                if (!_keys.Contains(key))
                    _keys.Add(key);
            }

            return _store.Set(prefixedKey, value, entryOptions);
        }

        public virtual async Task<object> Get(string key)
        {
            Condition.Requires(key, nameof(key)).IsNotNull();

            var prefixedKey = CreateKey(key);
            var value = await _store.Get(prefixedKey);

            if (value == null)
                Interlocked.Increment(ref _misses);
            else
                Interlocked.Increment(ref _hits);

            return value;
        }

        public virtual Task Remove(string key)
        {
            Condition.Requires(key, nameof(key)).IsNotNull();

            var prefixedKey = CreateKey(key);
            return _store.Remove(prefixedKey);
        }

        public Task Remove(string[] keys)
        {
            Condition.Requires(keys, nameof(keys)).IsNotNull();

            return _store.Remove(keys.Select(CreateKey).ToArray());
        }

        public virtual void SetCacheDefaultPriority(CacheEntryPriority defaultPriority)
        {
            _defaultPriority = defaultPriority;
        }

        public virtual Task<CacheStats> GetCacheStats()
        {
            return Task.FromResult(new CacheStats(_hits, _misses));
        }

        public Task ClearStats()
        {
            _misses = _hits = 0;
            return Task.FromResult((object) null);
        }

        protected virtual string CreateKey(string key)
        {
            return $"{Name}.{key}";
        }

        public Task Clear()
        {
            lock (_keysLock)
            {
                Remove(_keys.ToArray());
                ClearStats();
                _keys.Clear();
            }
            return Task.FromResult((object)null);
        }
    }
}