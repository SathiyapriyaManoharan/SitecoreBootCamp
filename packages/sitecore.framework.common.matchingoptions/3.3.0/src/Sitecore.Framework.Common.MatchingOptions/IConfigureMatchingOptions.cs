namespace Sitecore.Framework.Common.MatchingOptions
{
    public interface IConfigureMatchingOptions<TOptions>
    {
        string Identifier { get; }

        TOptions Configure(TOptions options, string name);
    }
}
