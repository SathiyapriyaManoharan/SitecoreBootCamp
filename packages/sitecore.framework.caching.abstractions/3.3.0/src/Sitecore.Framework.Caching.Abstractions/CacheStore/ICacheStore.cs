using System;
using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface ICacheStore : IDisposable
    {
        void Initialize();

        string Name { get; }

        Task Set(string key, ICachable value, CacheEntryOptions entryOptions);

        Task<object> Get(string key);

        Task Remove(string key);

        Task Remove(string[] keys);
    }
}