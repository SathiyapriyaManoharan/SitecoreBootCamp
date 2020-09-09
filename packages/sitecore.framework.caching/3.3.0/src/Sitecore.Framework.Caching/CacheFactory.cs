using Common;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Caching
{
    public class CacheFactory : ICacheFactory
    {
        protected readonly ICacheManager _cacheManager;

        public CacheFactory(ICacheManager cacheManager)
        {
            Condition.Requires(cacheManager, nameof(cacheManager)).IsNotNull();
            _cacheManager = cacheManager;
        }

        public ICache CreateCache(string cacheName)
        {
            Condition.Requires(cacheName, nameof(cacheName)).IsNotNull();
            return _cacheManager.CreateCache(cacheName);
        }

        public ICache CreateCache<T>(string suffix = "")
        {
            return CreateCache(TypeNameHelper.GetTypeDisplayName(typeof (T), fullName: true) + suffix);
        }
    }
}