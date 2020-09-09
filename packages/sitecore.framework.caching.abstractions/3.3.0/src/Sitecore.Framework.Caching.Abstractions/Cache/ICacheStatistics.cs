using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface ICacheStatistics
    {
        Task<CacheStats> GetCacheStats();
        Task ClearStats();
    }
}