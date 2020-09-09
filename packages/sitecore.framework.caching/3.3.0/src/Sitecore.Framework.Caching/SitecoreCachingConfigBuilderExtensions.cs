using System.Collections.Generic;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Caching
{
    using Common;

    public static class SitecoreCachingConfigBuilderExtensions
    {
        public static SitecoreCachingConfigBuilder ConfigureCaches(
           this SitecoreCachingConfigBuilder configBuilder,
           WildcardMatch cacheNamePattern,
           string storeName,
           CacheEntryPriority? defaultEntryPriority = null)
        {
            Condition.Requires(cacheNamePattern, nameof(cacheNamePattern)).IsNotNull();
            Condition.Requires(storeName, nameof(storeName)).IsNotNullOrWhiteSpace();

            return configBuilder.ConfigureCaches(cacheNamePattern, o =>
            {
                o.StoreName = storeName;
                if (defaultEntryPriority != null) o.EntryPriority = defaultEntryPriority.Value;
            });
        }

        public static SitecoreCachingConfigBuilder ConfigureCaches(
            this SitecoreCachingConfigBuilder configBuilder,
            IEnumerable<string> cacheNamePatterns,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            Condition.Requires(cacheNamePatterns, nameof(cacheNamePatterns)).IsNotNull();

            foreach (var cacheNamePattern in cacheNamePatterns)
            {
                configBuilder.ConfigureCaches(cacheNamePattern, storeName, defaultEntryPriority);
            }
            return configBuilder;
        }

        public static SitecoreCachingConfigBuilder ConfigureCache<T>(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            return configBuilder.ConfigureCaches(WildcardMatch.Named<T>(), storeName, defaultEntryPriority);
        }

        public static SitecoreCachingConfigBuilder ConfigureCaches<T1, T2>(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            configBuilder.ConfigureCache<T1>(storeName, defaultEntryPriority);
            return configBuilder.ConfigureCache<T2>(storeName, defaultEntryPriority);
        }

        public static SitecoreCachingConfigBuilder ConfigureCaches<T1, T2, T3>(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            configBuilder.ConfigureCache<T1>(storeName, defaultEntryPriority);
            configBuilder.ConfigureCache<T2>(storeName, defaultEntryPriority);
            return configBuilder.ConfigureCache<T3>(storeName, defaultEntryPriority);
        }

        public static SitecoreCachingConfigBuilder ConfigureCaches<T1, T2, T3, T4>(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            CacheEntryPriority? defaultEntryPriority = null)
        {
            configBuilder.ConfigureCache<T1>(storeName, defaultEntryPriority);
            configBuilder.ConfigureCache<T2>(storeName, defaultEntryPriority);
            configBuilder.ConfigureCache<T3>(storeName, defaultEntryPriority);
            return configBuilder.ConfigureCache<T4>(storeName, defaultEntryPriority);
        }
    }
}
