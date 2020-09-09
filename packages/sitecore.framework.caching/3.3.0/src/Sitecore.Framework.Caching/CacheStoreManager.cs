using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Caching
{
    public class CacheStoreManager : ICacheStoreManager
    {
        protected readonly IReadOnlyDictionary<string, ICacheStore> _stores;

        public CacheStoreManager(IEnumerable<ICacheStore> stores)
        {
            Condition.Requires(stores, nameof(stores)).IsNotNull();

            _stores = stores
                .Reverse()
                .Distinct(new CacheStoreEqualityByNameComparer())
                .ToDictionary(cp => cp.Name, cp => cp, StringComparer.InvariantCultureIgnoreCase);

            // TODO: consider case insensitive store names?
            // TODO: log warning message for duplicate store names?
        }

        public ICacheStore GetStore(string storeName)
        {
            Condition.Requires(storeName, nameof(storeName)).IsNotNull();
            ICacheStore store;
            return (_stores.TryGetValue(storeName, out store)) ? store : null;
        }

        public virtual async Task ClearAll()
        {
            foreach (var cacheStore in _stores.Values)
            {
                var value = cacheStore as IClearableCacheStore;

                if (value == null) continue;

                await value.Clear();
            }
        }

        public virtual async Task<CacheStoreStats> GetStatistics(string storeName)
        {
            Condition.Requires(storeName, nameof(storeName)).IsNotNull();

            ICacheStore store;
            if ((store = GetStore(storeName)) != null)
            {
                var cacheStoreStats = (store as ICacheStoreStatistics)?.GetStatistics();
                if (cacheStoreStats != null)
                    return await cacheStoreStats;
            }
            return null;
        }

        public virtual async Task<IDictionary<ICacheStore, CacheStoreStats>> GetAllStatistics()
        {
            var storeStats = new Dictionary<ICacheStore, CacheStoreStats>();

            foreach (var storeObj in _stores.Values
                .Select(s => new { name = s.Name, store = s as ICacheStoreStatistics }))
            {
                CacheStoreStats stats = null;
                if (storeObj.store != null) stats = await storeObj.store?.GetStatistics();
                storeStats.Add((ICacheStore)storeObj.store, stats);
            }

            return storeStats;
        }

        // IEnumerable

        public IEnumerator<ICacheStore> GetEnumerator()
        {
            return _stores.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _stores.Values.GetEnumerator();
        }

        private class CacheStoreEqualityByNameComparer : IEqualityComparer<ICacheStore>
        {
            public bool Equals(ICacheStore x, ICacheStore y)
            {
                return x != null && y != null ? x.Name.Equals(y.Name) : false;
            }

            public int GetHashCode(ICacheStore obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
