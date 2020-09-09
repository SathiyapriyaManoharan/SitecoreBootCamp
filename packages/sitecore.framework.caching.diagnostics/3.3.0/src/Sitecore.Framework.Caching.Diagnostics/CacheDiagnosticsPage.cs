using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Sitecore.Framework.Diagnostics;

namespace Sitecore.Framework.Caching.Diagnostics
{
    [DiagnosticsPageMetadata("Cache Diagnostics", "Use this page to view information about caches and cache stores")]
    public class CacheDiagnosticsPage : GenericDiagnosticPage
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheStoreManager _cacheStoreManager;

        public CacheDiagnosticsPage(ICacheManager cacheManager, ICacheStoreManager cacheStoreManager) : base("/admin/cache")
        {
            _cacheManager = cacheManager;
            _cacheStoreManager = cacheStoreManager;
        }

        /// <summary>
        /// Lists all the cache stores => /admin/cache
        /// </summary>
        public override object GetSummary()
        {
            var values = _cacheStoreManager.Select(x => new { CacheStoreName = x.Name, Type = x.GetType().Name, Url = $"{DefaultPath}/Store/{x.Name}" }).ToList(); // todo: url encode
            values.Add(new {CacheStoreName = "All", Type = string.Empty, Url = $"{DefaultPath}/All"});
                        //GetAllStats();
            return values;

        }

        /// <summary>
        /// Maps the request url to json data
        /// /admin/cache/store/{storename} returns information about a given store name
        /// /admin/cache/instance/{cachename} returns information about a given cache instance
        /// /admin/cache/all returns all the information about the available cache stores and caches
        /// </summary>
        public override object GetDetails(string category, string[] ids)
        {
            switch (category.ToLowerInvariant())
            {
                case "store":
                    if (ids != null && ids.Any())
                        return GetStoreDetails(ids[0]);
                    break;
                case "instance":
                    if (ids != null && ids.Any())
                        return GetCacheDetails(ids[0]);
                    break;
                case "all":
                    return GetAllStats();
            }
            return base.GetDetails(category, ids);
        }

        protected object GetStoreDetails(string storeName)
        {
            var store = _cacheStoreManager.GetStore(storeName);
            var storeStats = _cacheStoreManager.GetStatistics(storeName).Result;
            var cachesInStore = _cacheManager.GetCaches().Result.Where(x => string.Equals(x.StoreName, storeName, StringComparison.InvariantCultureIgnoreCase));
            return new
            {
                StoreName = store.Name,
                Type = store.GetType().Name,
                storeStats.ItemsCount,
                storeStats.MaxSize,
                storeStats.Size,
                Caches = cachesInStore.Select(x => new {CacheName = x.Name, Url = $"{DefaultPath}/Instance/{x.Name}"})
            };
        }

        protected object GetCacheDetails(string cacheName)
        {
            var cache = _cacheManager.GetCache(cacheName);
            var cacheStats = _cacheManager.GetStatistics(cacheName).Result;
            return new {CacheName = cache.Name, cacheStats.Hits, cacheStats.Misses};
        }

        protected object GetAllStats()
        {
            return _cacheStoreManager.Select(store => new
            {
                StoreName = store.Name,
                Stats = _cacheStoreManager.GetStatistics(store.Name).Result,
                Caches =
                    _cacheManager.GetCaches()
                        .Result.Where(cache => cache.StoreName == store.Name)
                        .Select(
                            cache =>
                                new {CacheName = cache.Name, Stats = _cacheManager.GetStatistics(cache.Name).Result})
            });
        }
    }
}
