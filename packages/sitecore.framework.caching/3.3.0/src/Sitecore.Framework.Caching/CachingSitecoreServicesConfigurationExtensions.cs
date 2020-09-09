using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Framework.Caching;
using Sitecore.Framework.Common.MatchingOptions;
using Sitecore.Framework.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CachingSitecoreServicesConfigurationExtensions
    {
        /// <summary>
        /// Adds the Sitecore caching system to the service collection. Only to be called once per application.
        /// This function will setup a cache memory provider that will be used by all caches (by default) unless they are configured to use other providers.
        /// </summary>
        public static ISitecoreServicesConfiguration Caching(this ISitecoreServicesConfiguration builder, Action<SitecoreCachingConfigBuilder> configure = null)
        {
            builder.Services.AddMatchingOptions();

            builder.Services.TryAdd(ServiceDescriptor.Singleton<ICacheManager, CacheManager>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ICache<>), typeof(Cache<>)));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<ICacheStoreManager, CacheStoreManager>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<ICacheFactory, CacheFactory>());
            builder.Services.TryAddSingleton<ICacheStoreSerializer, DefaultCacheStoreSerializer>();

            // Ensure default options are set if this is the first call in startup.
            builder.Services.TryAdd(ServiceDescriptor.Transient<IConfigureMatchingOptions<CacheOptions>, DefaultCacheOptionsSetup>());

            var configBuilder = new SitecoreCachingConfigBuilder(builder);

            configure?.Invoke(configBuilder);

            return builder;
        }
    }
}