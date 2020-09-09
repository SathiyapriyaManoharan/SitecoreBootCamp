using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface ICacheStoreStatistics
    {
        Task<CacheStoreStats> GetStatistics();
    }
}