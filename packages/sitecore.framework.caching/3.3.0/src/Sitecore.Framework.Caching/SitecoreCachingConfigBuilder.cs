using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Caching
{
    using Common;
    using Common.MatchingOptions;
    using Configuration;

    public class SitecoreCachingConfigBuilder
    {
        private readonly ISitecoreServicesConfiguration _sitecore;

        public SitecoreCachingConfigBuilder(ISitecoreServicesConfiguration sitecore)
        {
            Condition.Requires(sitecore, nameof(sitecore)).IsNotNull();

            _sitecore = sitecore;
            TryConfigureCacheDefault(cache => cache);
        }

        public IServiceCollection Services => _sitecore.Services;

        public SitecoreCachingConfigBuilder ConfigureCaches(
            string cacheNamePattern,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            Condition.Requires(cacheNamePattern, nameof(cacheNamePattern)).IsNotNull();

            var parsed = WildcardMatch.Parse(cacheNamePattern);
            return ConfigureCaches(parsed, o =>
            {
                o.StoreName = storeName;
                if (defaultEntryPriority != null) o.EntryPriority = defaultEntryPriority.Value;
            });
        }

        public SitecoreCachingConfigBuilder ConfigureCaches(
            WildcardMatch cacheNamePattern,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            Condition.Requires(cacheNamePattern, nameof(cacheNamePattern)).IsNotNull();
            
            return ConfigureCaches(cacheNamePattern, o =>
            {
                o.StoreName = storeName;
                if (defaultEntryPriority != null) o.EntryPriority = defaultEntryPriority.Value;
            });
        }

        public SitecoreCachingConfigBuilder ConfigureCaches(
            WildcardMatch cacheNamePattern,
            Action<CacheOptions> configureCache)
        {
            Condition.Requires(cacheNamePattern, nameof(cacheNamePattern)).IsNotNull();

            _sitecore.Services.ConfigureMatching(cacheNamePattern.Identifier, cacheNamePattern, configureCache);

            return this;
        }

        public SitecoreCachingConfigBuilder ReConfigureAllCaches(Action<string, CacheOptions> reconfigureCache)
        {
            Condition.Requires(reconfigureCache, nameof(reconfigureCache)).IsNotNull();

            _sitecore.Services.ConfigureMatchingOptions(
                new ReconfigureAllMatchingOptions<CacheOptions>(Guid.NewGuid().ToString(), (cacheName, options) => {
                    reconfigureCache(cacheName, options); return options;
                }));

            return this;
        }

        public SitecoreCachingConfigBuilder ConfigureCacheDefaults(Action<CacheOptions> configureCache)
        {
            return ConfigureCaches(WildcardMatch.All(), configureCache);
        }

        public SitecoreCachingConfigBuilder ConfigureCacheDefaults(string storeName, CacheEntryPriority? defaultEntryPriority = null)
        {
            return ConfigureCacheDefaults(o =>
            {
                o.StoreName = storeName;
                if (defaultEntryPriority != null) o.EntryPriority = defaultEntryPriority.Value;
            });
        }

        public SitecoreCachingConfigBuilder SetDefaultSerializer<TDefaultSerializer>()
            where TDefaultSerializer : class, ICacheStoreSerializer
        {
            _sitecore.Services.Replace(ServiceDescriptor.Singleton<ICacheStoreSerializer, TDefaultSerializer>());
            return this;
        }

        private void TryConfigureCacheDefault(Func<CacheOptions, CacheOptions> configure)
        {
            var match = WildcardMatch.All();
            _sitecore.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureMatchingOptions<CacheOptions>>(
                new ConfigureMatchingOptions<CacheOptions>(match.Identifier, match, configure)));
        }
    }
}
