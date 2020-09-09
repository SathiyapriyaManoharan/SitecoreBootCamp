using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface ICache
    {
        string StoreName { get; }

        string Name { get; }

        Task Set(string key, ICachable value, CacheEntryOptions entryOptions = null);

        Task<object> Get(string key);

        Task Remove(string key);

        Task Remove(string[] keys);

        Task Clear();
    }
}