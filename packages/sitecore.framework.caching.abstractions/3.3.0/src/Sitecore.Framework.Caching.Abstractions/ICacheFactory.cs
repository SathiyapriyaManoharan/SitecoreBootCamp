namespace Sitecore.Framework.Caching
{
    public interface ICacheFactory
    {
        ICache CreateCache(string cacheName);

        ICache CreateCache<T>(string suffix = "");
    }
}