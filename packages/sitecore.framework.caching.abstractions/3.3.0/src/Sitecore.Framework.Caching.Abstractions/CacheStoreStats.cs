namespace Sitecore.Framework.Caching
{
    public class CacheStoreStats
    {
        public long? MaxSize { get; }
        public long? Size { get; }
        public int? ItemsCount { get; }

        public CacheStoreStats(long? maxSize, long? size, int? itemsCount)
        {
            MaxSize = maxSize;
            Size = size;
            ItemsCount = itemsCount;
        }
    }
}