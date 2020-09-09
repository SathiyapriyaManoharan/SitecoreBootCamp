using System;

namespace Sitecore.Framework.Common.MatchingOptions
{
    public class ReconfigureAllMatchingOptions<TOptions> : IConfigureMatchingOptions<TOptions>
    {
        private readonly Func<string, TOptions, TOptions> _reconfigure;

        public ReconfigureAllMatchingOptions(string identifier, Func<string, TOptions, TOptions> reconfigure)
        {
            Identifier = identifier;
            _reconfigure = reconfigure;
        }

        public string Identifier { get; }

        public TOptions Configure(TOptions options, string name)
        {
            return _reconfigure(name, options);
        }
    }
}
