using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface ICacheManager
    {
        ICache CreateCache(string cacheName);

        ICache GetCache(string cacheName);

        Task<CacheStats> GetStatistics(string cacheName);

        Task ClearAll();

        Task<IDictionary<ICache, CacheStats>> GetAllStatistics();

        Task<IEnumerable<ICache>> GetCaches();
    }
}