using System;

namespace Sitecore.Framework.Caching
{
    public class CacheEntryOptions
    {
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
       
        public TimeSpan? SlidingExpiration { get; set; }

        public CacheEntryPriority? Priority { get; set; }
    }

    public enum CacheEntryPriority {
        Low = 0,
        Normal = 1,
        High = 2,
        NeverRemove = 3
    }
}