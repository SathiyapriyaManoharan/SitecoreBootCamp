namespace Sitecore.Framework.Caching
{
    using Microsoft.Extensions.DependencyInjection;

    using Sitecore.Framework.Configuration;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(ISitecoreServicesConfiguration sitecore)
        {
            sitecore.Caching();
        }
    }
}