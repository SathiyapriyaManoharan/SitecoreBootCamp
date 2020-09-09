namespace Sitecore.Framework.Caching
{
    public class CacheOptions
    {
        public string StoreName { get; set; }

        public SitecoreCacheFactoryDelegate CacheFactory { get; set; }

        public CacheEntryPriority EntryPriority { get; set; } = CacheEntryPriority.Normal;
    }
}