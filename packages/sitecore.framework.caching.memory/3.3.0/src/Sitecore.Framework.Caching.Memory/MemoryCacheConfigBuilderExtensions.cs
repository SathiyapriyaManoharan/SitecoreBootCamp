using System;
using Sitecore.Framework.Caching;
using Sitecore.Framework.Caching.Memory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MemoryCacheConfigBuilderExtensions
    {
        public static SitecoreCachingConfigBuilder AddMemoryStore(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            Action<MemoryCacheStoreOptions> configureStore = null)
        {
            if (configureStore != null)
                configBuilder.Services.ConfigureMatching<MemoryCacheStoreOptions>(configureStore, storeName);

            configBuilder.Services.AddSingleton<ICacheStore>(sp =>
            {
                return ActivatorUtilities.CreateInstance<MemoryCacheStore>(sp, storeName);
            });

            return configBuilder;
        }

        public static SitecoreCachingConfigBuilder SetDefaultMemoryStoreSettings(
            this SitecoreCachingConfigBuilder configBuilder,
            Action<MemoryCacheStoreOptions> configureStore)
        {
            configBuilder.Services.Configure<MemoryCacheStoreOptions>(configureStore);
            return configBuilder;
        }
    }
}