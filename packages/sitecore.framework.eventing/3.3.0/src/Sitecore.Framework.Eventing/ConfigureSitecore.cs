namespace Eventing
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Sitecore.Framework.Configuration;

    public class ConfigureSitecore : IConfigureSitecore
    {
        private readonly ILogger<ConfigureSitecore> _logger;

        public ConfigureSitecore(ILogger<ConfigureSitecore> logger)
        {
            _logger = logger;
        }

        public void ConfigureServices(ISitecoreServicesConfiguration sitecore)
        {
            _logger.LogInformation("Configuring event defaults");
            sitecore.Eventing();
        }
    }
}