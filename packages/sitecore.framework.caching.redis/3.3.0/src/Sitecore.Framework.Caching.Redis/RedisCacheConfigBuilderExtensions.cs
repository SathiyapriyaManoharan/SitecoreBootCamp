using System;
using Sitecore.Framework.Caching;
using Microsoft.Extensions.Caching.Redis;
using Sitecore.Framework.Caching.Redis;
using Sitecore.Framework.Conditions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisSitecoreCachingConfigBuilderExtensions
    {
        public static SitecoreCachingConfigBuilder AddRedisStore(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            string redisConfiguration,
            string redisInstanceName)
        {
            AddRedisStore(configBuilder, storeName, redisConfiguration, redisInstanceName, sp => sp.GetRequiredService<ICacheStoreSerializer>());
            return configBuilder;
        }

        public static SitecoreCachingConfigBuilder AddRedisStore<TSerializer>(
            this SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            string redisConfiguration,
            string redisInstanceName)
             where TSerializer : class, ICacheStoreSerializer
        {
            AddRedisStore(configBuilder, storeName, redisConfiguration, redisInstanceName, sp => ActivatorUtilities.CreateInstance<TSerializer>(sp));
            return configBuilder;
        }

        private static void AddRedisStore<TSerializer>(
            SitecoreCachingConfigBuilder configBuilder,
            string storeName,
            string redisConfiguration,
            string redisInstanceName,
            Func<IServiceProvider, TSerializer> serializerFactory)
             where TSerializer : class, ICacheStoreSerializer
        {
            Condition.Requires(storeName, nameof(storeName)).IsNotNullOrEmpty();
            Condition.Requires(redisConfiguration, nameof(redisConfiguration)).IsNotNull();
            Condition.Requires(redisInstanceName, nameof(redisInstanceName)).IsNotNull();

            configBuilder.Services.ConfigureMatching<RedisCacheOptions>(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = redisInstanceName;
            }, storeName);

            configBuilder.Services.AddSingleton<ICacheStore>(sp =>
            {
                return ActivatorUtilities.CreateInstance<RedisCacheStore>(sp, serializerFactory(sp), storeName);
            });
        }
    }
}