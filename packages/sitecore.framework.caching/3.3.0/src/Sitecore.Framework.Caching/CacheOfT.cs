using System;
using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    // todo: are these seperate cache interfaces needed anymore?

    public class Cache<T> : ICache<T>, IPrioritizableCache, ICacheStatistics
    {
        private readonly ICache _cache;

        public Cache(ICacheFactory cacheFactory)
        {
            _cache = cacheFactory.CreateCache<T>();
        }

        public string Name => _cache.Name;

        public string StoreName => _cache.StoreName;

        public Task Clear()
        {
            return _cache.Clear();
        }

        public Task ClearStats()
        {
            return ((ICacheStatistics)_cache).ClearStats();
        }

        public Task<object> Get(string key)
        {
            return _cache.Get(key);
        }

        public Task<CacheStats> GetCacheStats()
        {
            return ((ICacheStatistics)_cache).GetCacheStats();
        }

        public Task Remove(string[] keys)
        {
            return _cache.Remove(keys);
        }

        public Task Remove(string key)
        {
            return _cache.Remove(key);
        }

        public Task Set(string key, ICachable value, CacheEntryOptions entryOptions = null)
        {
            return _cache.Set(key, value, entryOptions);
        }

        public void SetCacheDefaultPriority(CacheEntryPriority defaultPriority)
        {
            ((IPrioritizableCache)_cache).SetCacheDefaultPriority(defaultPriority);
        }
    }
}
