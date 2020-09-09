using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Common.MatchingOptions;

namespace Sitecore.Framework.Caching
{
    public class CacheManager : ICacheManager
    {
        protected readonly ILogger _logger;

        protected readonly IMatchingOptions<CacheOptions> _cacheOptions;

        protected readonly ConcurrentDictionary<string, ICache> _caches = 
            new ConcurrentDictionary<string, ICache>(StringComparer.InvariantCultureIgnoreCase);

        protected readonly ICacheStoreManager _storeRegistry;

        public CacheManager(
            ILogger<CacheManager> logger, 
            ICacheStoreManager storeRegistry,
            IMatchingOptions<CacheOptions> cacheOptions)
        {
            Condition.Requires(cacheOptions, nameof(cacheOptions)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            Condition.Requires(storeRegistry, nameof(storeRegistry)).IsNotNull();

            _logger = logger;
            _cacheOptions = cacheOptions;
            _storeRegistry = storeRegistry;
        }

        public virtual ICache CreateCache(string cacheName)
        {
            Condition.Requires(cacheName, nameof(cacheName)).IsNotNullOrWhiteSpace();

            var cacheOptions = _cacheOptions.GetMatchingOptions(cacheName);

            if (string.IsNullOrEmpty(cacheOptions.StoreName))
                throw new ArgumentOutOfRangeException(nameof(cacheName), $"Cannot create cache '{cacheName}'.  No cache store has been configured.");

            var store = _storeRegistry.GetStore(cacheOptions.StoreName);

            if (store == null)
                throw new ArgumentException($"Cannot create cache '{cacheName}'.  Cache store '{cacheOptions.StoreName}' has not been configured.");

            var cache = _caches.GetOrAdd(cacheName, key =>
            {
                _logger.LogDebug("Creating cache {CacheName} with store '{CacheStoreName}'", cacheName, store.Name);
                store.Initialize();
                var newCache = cacheOptions.CacheFactory(store, cacheName);
                (newCache as IPrioritizableCache)?.SetCacheDefaultPriority(cacheOptions.EntryPriority);
                return newCache;
            });

            return cache;
        }

        public virtual ICache GetCache(string cacheName)
        {
            Condition.Requires(cacheName, nameof(cacheName)).IsNotNullOrWhiteSpace();

            ICache cache;
            _caches.TryGetValue(cacheName.ToLowerInvariant(), out cache);
            return cache;
        }
        public virtual async Task<CacheStats> GetStatistics(string cacheName)
        {
            Condition.Requires(cacheName, nameof(cacheName)).IsNotNull();

            ICache cache;
            if (_caches.TryGetValue(cacheName, out cache))
            {
                var cacheStats = (cache as ICacheStatistics)?.GetCacheStats();
                if (cacheStats != null)
                    return await cacheStats;
            }
            return null;
        }

        public virtual async Task<IDictionary<ICache, CacheStats>> GetAllStatistics()
        {
            var cachesStats = new Dictionary<ICache, CacheStats>();
            foreach (var cache in _caches)
            {
                var stats = await GetStatistics(cache.Key);
                if (stats != null)
                {
                    cachesStats.Add(cache.Value, stats);
                }
            }
            return cachesStats;
        }

        public virtual async Task ClearAll()
        {
            await _storeRegistry.ClearAll();

            foreach (var cache in _caches.OfType<ICacheStatistics>())
            {
                await cache.ClearStats();
            }
        }

        public virtual Task<IEnumerable<ICache>> GetCaches()
        {
            return Task.FromResult((IEnumerable<ICache>) _caches.Values);
        }
    }
}