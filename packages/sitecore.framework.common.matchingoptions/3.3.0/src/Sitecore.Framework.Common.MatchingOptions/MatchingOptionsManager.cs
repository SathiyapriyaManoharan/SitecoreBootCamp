using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Framework.Common.MatchingOptions
{
    public class MatchingOptionsManager<TOptions> : IMatchingOptions<TOptions> where TOptions : class, new()
    {
        private object _cacheLock = new object();
        private Dictionary<string, TOptions> _optionsCache = new Dictionary<string, TOptions>(StringComparer.OrdinalIgnoreCase);
        private IEnumerable<IConfigureMatchingOptions<TOptions>> _setups;

        public MatchingOptionsManager(IEnumerable<IConfigureMatchingOptions<TOptions>> setups)
        {
            _setups = setups == null ?
                null :
                setups.GroupBy(s => s.Identifier, s => s)
                    .SelectMany(g => g)
                    .ToArray();
        }

        TOptions IOptions<TOptions>.Value => GetMatchingOptions(string.Empty);

        public virtual TOptions GetMatchingOptions(string name)
        {
            if (!_optionsCache.ContainsKey(name))
            {
                lock (_cacheLock)
                {
                    if (!_optionsCache.ContainsKey(name))
                    {
                        _optionsCache[name] = Configure(name);
                    }
                }
            }
            return _optionsCache[name];
        }

        protected TOptions Configure(string optionsName = "")
        {
            return _setups == null ?
                new TOptions() :
                _setups.Aggregate(new TOptions(), (options, setup) => setup.Configure(options, optionsName));
        }
    }
}