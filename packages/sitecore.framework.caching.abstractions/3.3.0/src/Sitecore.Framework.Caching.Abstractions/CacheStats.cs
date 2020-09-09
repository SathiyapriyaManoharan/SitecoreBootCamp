namespace Sitecore.Framework.Caching
{
    public class CacheStats
    {
        public long Hits { get; }
        public long Misses { get; }
        
        public CacheStats(long hits, long misses)
        {
            Hits = hits;
            Misses = misses;
        }
        
    }
}