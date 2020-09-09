using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Common.MatchingOptions;

namespace Sitecore.Framework.Caching.Redis
{
    public class RedisCacheStore : ICacheStore
    {
        protected readonly ILogger _logger;

        protected readonly ICacheStoreSerializer _objectSerializer;

        protected readonly RedisCacheOptions _options;

        protected RedisCache _redisCache;

        public RedisCacheStore(
            ILoggerFactory loggerFactory,
            ICacheStoreSerializer objectSerializer, 
            IMatchingOptions<RedisCacheOptions> options,
            string name)
        {
            Condition.Requires(loggerFactory, nameof(loggerFactory)).IsNotNull();
            Condition.Requires(options, nameof(options)).IsNotNull();
            Condition.Requires(objectSerializer, nameof(objectSerializer)).IsNotNull();

            Name = name;
            _options = options.GetMatchingOptions(Name);
            _objectSerializer = objectSerializer;
            _logger = loggerFactory.CreateLogger($"{name}-{nameof(RedisCacheStore)}");
        }

        public void Initialize()
        {
            if (_redisCache == null)
            {
                _redisCache = CreateCache();
            }
        }

        public virtual async Task Set(string key, ICachable value, CacheEntryOptions entryOptions)
        {
            Condition.Requires(key, nameof(key));
            Condition.Requires(value, nameof(value));
            Condition.Requires(entryOptions, nameof(entryOptions));

            var distributedCacheEntryOptions = CreateDistributedCacheEntryOptions(entryOptions);
            var serializedValue = _objectSerializer.Serialize(value.Value);
            await _redisCache.SetAsync(key, serializedValue, distributedCacheEntryOptions);
            _logger.LogDebug("{Operation} => key : {Key} | value : {Value}", "set", key, value.Value);
        }

        public virtual async Task<object> Get(string key)
        {
            Condition.Requires(key, nameof(key));

            var serializedData = await _redisCache.GetAsync(key);
            object value = null;
            if (serializedData != null)
            {
                value = _objectSerializer.Deserialize(serializedData);
                _logger.LogDebug("{Operation} => key : {Key} | value : {Value}", "get", key, value);
            }
            else
            {
                _logger.LogDebug("{Operation} => key '{Key}' not found", "get", key);
            }
            
            return value;
        }

        public virtual Task Remove(string key)
        {
            Condition.Requires(key, nameof(key));

            _logger.LogDebug("{OperationResult} => key : {Key}", "remove", key);
            return _redisCache.RemoveAsync(key);
        }

        public async Task Remove(string[] keys)
        {
            foreach (var key in keys)
            {
                await Remove(key);
            }
        }

        public virtual void Dispose()
        {

        }

        public string Name { get; }

        protected virtual RedisCache CreateCache()
        {
            _logger.LogInformation("Creating Redis cache store '{CacheStoreName}'", Name);
            return new RedisCache(_options);
        }

        private DistributedCacheEntryOptions CreateDistributedCacheEntryOptions(CacheEntryOptions entryOptions)
        {
            var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpiration = entryOptions.AbsoluteExpiration;
            distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = entryOptions.AbsoluteExpirationRelativeToNow;
            distributedCacheEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration;
            return distributedCacheEntryOptions;
        }
    }
}