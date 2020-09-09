namespace Sitecore.Framework.Caching
{
    using Common;
    using Common.MatchingOptions;

    public class DefaultCacheOptionsSetup : ConfigureMatchingOptions<CacheOptions>
    {
        private static readonly WildcardMatch Matcher = WildcardMatch.All();

        public DefaultCacheOptionsSetup() :
            base(Matcher.Identifier, Matcher, ConfigureCaching)
        {
        }

        public static CacheOptions ConfigureCaching(CacheOptions options)
        {
            options.EntryPriority = CacheEntryPriority.Normal;
            options.CacheFactory = (store, cacheName) => new Cache(store, cacheName);
            return options;
        }
    }
}
