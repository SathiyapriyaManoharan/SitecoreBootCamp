using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface ICacheStoreManager : IEnumerable<ICacheStore>
    {
        ICacheStore GetStore(string storeName);

        Task ClearAll();

        Task<CacheStoreStats> GetStatistics(string storeName);

        Task<IDictionary<ICacheStore, CacheStoreStats>> GetAllStatistics();
    }
}
