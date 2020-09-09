using Microsoft.Extensions.DependencyInjection;

namespace Sitecore.Framework.Eventing
{
    using Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class SitecoreEventingConfigBuilder
    {
        public ISitecoreServicesConfiguration Sitecore { get; }

        public SitecoreEventingConfigBuilder(ISitecoreServicesConfiguration sitecore)
        {
            Sitecore = sitecore;
        }

        public SitecoreEventingConfigBuilder AddDistributor<TDistributor>()
            where TDistributor : class, IDistributor
        {
            Sitecore.Services.AddSingleton<IDistributor, TDistributor>();
            return this;
        }

        public SitecoreEventingConfigBuilder SetEventRegistry<TRegistry>()
            where TRegistry : class, IEventRegistry
        {
            Sitecore.Services.Replace(ServiceDescriptor.Singleton<IEventRegistry, TRegistry>());
            return this;
        }
    }
}
