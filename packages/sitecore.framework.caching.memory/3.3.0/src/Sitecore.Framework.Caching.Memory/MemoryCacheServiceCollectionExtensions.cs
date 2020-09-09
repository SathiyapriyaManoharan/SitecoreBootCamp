using Sitecore.Framework.Caching.Memory;
using Sitecore.Framework.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MemoryCacheServiceCollectionExtensions
    {
        public static ISitecoreServicesConfiguration CachingWithMemoryDefaults(
            this ISitecoreServicesConfiguration builder, 
            Action<MemoryCacheStoreOptions> configureStore = null,
            string defaultStoreName = "Memory-Default")
        {
            builder.Caching(config => config
                .AddMemoryStore(defaultStoreName, configureStore)
                .ConfigureCacheDefaults(defaultStoreName));

            return builder;
        }
    }
}
