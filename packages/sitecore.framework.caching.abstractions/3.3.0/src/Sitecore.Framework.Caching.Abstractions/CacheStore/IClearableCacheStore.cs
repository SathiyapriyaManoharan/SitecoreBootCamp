using System.Threading.Tasks;

namespace Sitecore.Framework.Caching
{
    public interface IClearableCacheStore
    {
        Task Clear();
    }
}