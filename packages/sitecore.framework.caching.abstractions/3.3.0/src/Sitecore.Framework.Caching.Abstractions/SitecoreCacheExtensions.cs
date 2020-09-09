using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public static class SitecoreCacheExtensions
    {
        public static Task SetString(this ICache cache,  string key, string value, CacheEntryOptions entryOptions = null)
        {
            var cachable = new Cachable<string>(value, Encoding.Unicode.GetByteCount(value));
            return cache.Set(key, cachable, entryOptions);
        }

        public static Task SetInt(this ICache cache, string key, int value, CacheEntryOptions entryOptions = null)
        {
            var cachable = new Cachable<int>(value, sizeof(int));
            return cache.Set(key, cachable, entryOptions);
        }

        public static Task SetLong(this ICache cache, string key, long value, CacheEntryOptions entryOptions = null)
        {
            var cachable = new Cachable<long>(value, sizeof(long));
            return cache.Set(key, cachable, entryOptions);
        }

        public static async Task<T> Get<T>(this ICache cache, string key)
        {
            var obj = await cache.Get(key);
            return (T) obj;
        }
    }
}