using System;

namespace Sitecore.Framework.Common.MatchingOptions
{
    public class ConfigureMatchingOptions<TOptions> : IConfigureMatchingOptions<TOptions>
    {
        public ConfigureMatchingOptions(string identifier, Predicate<string> match, Func<TOptions, TOptions> setup)
        {
            Identifier = identifier;
            Match = match;
            Setup = setup;
        }

        public Predicate<string> Match { get; }

        public Func<TOptions, TOptions> Setup { get; }

        public string Identifier { get; }

        public TOptions Configure(TOptions options, string name)
        {
            return Match(name) ? Setup(options) : options;
        }
    }
}
