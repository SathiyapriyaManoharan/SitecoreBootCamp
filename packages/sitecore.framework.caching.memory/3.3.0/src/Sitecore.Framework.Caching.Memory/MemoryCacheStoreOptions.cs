using System;
using Microsoft.Extensions.Caching.Memory;
using Sitecore.Framework.Common.MatchingOptions;

namespace Sitecore.Framework.Caching.Memory
{
    public class MemoryCacheStoreOptions : MemoryCacheOptions, IMatchingOptions<MemoryCacheStoreOptions>
    {
        public double EvictionPercentage { get; set; } = 0.2;

        public TimeSpan PollingInterval { get; set; } = TimeSpan.FromMinutes(2);

        public long MaxSizeInBytes { get; set; } = long.MaxValue;

        public MemoryCacheStoreOptions GetMatchingOptions(string name)
        {
            return this;
        }

        public MemoryCacheStoreOptions Value => this;
    }
}