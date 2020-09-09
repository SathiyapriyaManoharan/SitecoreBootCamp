namespace Sitecore.Framework.Caching.Memory
{
    using Microsoft.Extensions.DependencyInjection;

    using Sitecore.Framework.Configuration;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(ISitecoreServicesConfiguration sitecore)
        {
            sitecore.CachingWithMemoryDefaults();
        }
    }
}