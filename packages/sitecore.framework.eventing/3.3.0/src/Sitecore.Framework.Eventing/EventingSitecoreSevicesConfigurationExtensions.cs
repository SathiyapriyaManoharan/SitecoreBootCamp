using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Eventing;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventingSitecoreSevicesConfigurationExtensions
    {
        public static ISitecoreServicesConfiguration Eventing(this ISitecoreServicesConfiguration builder, Action<SitecoreEventingConfigBuilder> configure = null)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IEventRegistry, EventRegistry>());

            configure?.Invoke(new SitecoreEventingConfigBuilder(builder));

            return builder;
        }
    }
}