using Microsoft.Extensions.Options;

namespace Sitecore.Framework.Common.MatchingOptions
{
    public interface IMatchingOptions<out TOptions> : IOptions<TOptions>
        where TOptions : class, new()
    {
        TOptions GetMatchingOptions(string name);
    }
}
