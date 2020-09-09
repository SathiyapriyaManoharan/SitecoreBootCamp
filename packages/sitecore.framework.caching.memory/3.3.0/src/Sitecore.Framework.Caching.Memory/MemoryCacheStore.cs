using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Common.MatchingOptions;

namespace Sitecore.Framework.Caching.Memory
{

    public class MemoryCacheStore : ICacheStore, IClearableCacheStore, ICacheStoreStatistics
    {
        protected readonly MemoryCacheStoreOptions _options;

        protected readonly ILogger _logger;

        protected readonly Timer _cleanupTimer;

        protected readonly ReaderWriterLockSlim _syncClearLock = new ReaderWriterLockSlim();

        protected static readonly Task CompletedTask = Task.FromResult<object>(null);

        protected MemoryCache _memoryCache;

        protected long _memoryUsed;

        protected long _maxSize;

        protected int _itemsCount;

        public MemoryCacheStore(
            ILoggerFactory loggerFactory, 
            IMatchingOptions<MemoryCacheStoreOptions> options,
            string name)
        {
            Condition.Requires(loggerFactory, nameof(loggerFactory)).IsNotNull();
            Condition.Requires(options, nameof(options)).IsNotNull();
            Condition.Requires(name, nameof(name)).IsNotNullOrEmpty();

            _options = options.GetMatchingOptions(name);
            _maxSize = _options.MaxSizeInBytes;
            Name = name;
            
            _logger = loggerFactory.CreateLogger($"{name}-{nameof(MemoryCacheStore)}");
            _cleanupTimer = new Timer(obj => CompactIfNeeded(), this, _options.PollingInterval, _options.PollingInterval); 
        }

        public virtual void Initialize()
        {
            if (_memoryCache == null)
            {
                _memoryCache = CreateCache();
            }
        }

        public virtual Task Set(string key, ICachable value, CacheEntryOptions entryOptions)
        {
            Condition.Requires(key, nameof(key)).IsNotNullOrEmpty();
            Condition.Requires(value, nameof(value)).IsNotNull();
            Condition.Requires(entryOptions, nameof(entryOptions)).IsNotNull();

            _syncClearLock.EnterReadLock();

            try
            {
                if (CheckSize(value))
                {
                    var memoryCacheEntryOptions = CreateMemoryCacheEntryOptions(entryOptions);

                    Interlocked.Add(ref _memoryUsed, value.SizeInBytes);
                    Interlocked.Increment(ref _itemsCount);

                    _memoryCache.Set(key, value, memoryCacheEntryOptions);

                    _logger.LogDebug("{Operation} => key : {Key} | value : {Value}", "set", key, value.Value);
                }
                else
                {
                    _logger.LogWarning("{CacheStore} Cache is full, cannot add item ({Key}) to cache.", Name, key);
                }
            }
            finally
            {
                _syncClearLock.ExitReadLock();

            }

            return CompletedTask;
        }

        public virtual Task<object> Get(string key)
        {
            Condition.Requires(key, nameof(key)).IsNotNull();

            _syncClearLock.EnterReadLock();

            try
            {
                object obj;
                if (_memoryCache.TryGetValue(key, out obj))
                {
                    var cachable = (ICachable)obj;
                    _logger.LogDebug("{Operation} => key : {Key} | value : {Value}", "get", key, cachable.Value.ToString());
                    return Task.FromResult(cachable.Value);
                }

                _logger.LogDebug("{Operation} => key '{Key}' not found", "get", key);
            }
            finally
            {
                _syncClearLock.ExitReadLock();

            }

            return Task.FromResult<object>(null);
        }

        public virtual Task Remove(string key)
        {
            Condition.Requires(key, nameof(key)).IsNotNull();

            _syncClearLock.EnterReadLock();

            try
            {

                var obj = _memoryCache.Get<ICachable>(key);
                if (obj != null)
                {
                    _memoryCache.Remove(key);
                    _logger.LogDebug("{OperationName} => key : {Key} {Result}", "remove", key, "success");
                }
                else
                {
                    _logger.LogDebug("{OperationName} => key : {Key} {Result}", "remove", key, "not found");
                }

            }
            finally
            {
                _syncClearLock.ExitReadLock();
            }

            return CompletedTask;
        }

        public virtual async Task Remove(string[] keys)
        {
            Condition.Requires(keys, nameof(keys)).IsNotNull();

            foreach (var key in keys)
            {
                await Remove(key);
            }
        }

        public virtual Task Clear()
        {
            _syncClearLock.EnterWriteLock();

            try
            {
                _memoryCache = CreateCache();
                _memoryUsed = _itemsCount = 0;
            }
            finally
            {
                _syncClearLock.ExitWriteLock();
            }

            return CompletedTask;

        }

        public virtual Task<CacheStoreStats> GetStatistics()
        {
            return Task.FromResult(new CacheStoreStats(_maxSize, _memoryUsed, _itemsCount));
        }

        public virtual void Dispose()
        {
            _memoryCache?.Dispose();
        }

        public string Name { get; }

        protected virtual MemoryCache CreateCache()
        {
            _logger.LogInformation("Creating Memory cache store '{CacheStoreName}'", Name);
            return new MemoryCache(_options);
        }

        protected virtual bool CheckSize(ICachable cachable)
        {
            if (cachable.SizeInBytes + _memoryUsed > _maxSize)
            {
                Compact();
                return false;
            }
            return true;

        }

        protected virtual void CompactIfNeeded()
        {
            if (_memoryUsed > _maxSize)
            {
                Compact();
            }
        }

        protected virtual void Compact()
        {
            _logger.LogDebug("Cleanup task started. Memory Used : {MemoryUsed}, Memory Limit = {MemoryLimit}", _memoryUsed, _maxSize);
            _memoryCache.Compact(_options.EvictionPercentage);
        }

        virtual protected void PostEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            
            var cachable = value as ICachable;
            Interlocked.Add(ref _memoryUsed, -cachable.SizeInBytes);
            Interlocked.Decrement(ref _itemsCount);
            
        }

        private MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheEntryOptions entryOptions)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            memoryCacheEntryOptions.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
            {
                EvictionCallback = PostEvictionCallback
            });
            memoryCacheEntryOptions.AbsoluteExpiration = entryOptions.AbsoluteExpiration;
            memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = entryOptions.AbsoluteExpirationRelativeToNow;
            memoryCacheEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration;
            return memoryCacheEntryOptions;
        }
    }
}