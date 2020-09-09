using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Sitecore.Framework.Diagnostics.Services
{
    public class SitecoreServiceDiagnostics : ISitecoreServiceDiagnostics
    {
        private readonly IServiceProvider _services;
        private readonly IServiceCollection _serviceCollection;

        public SitecoreServiceDiagnostics(IServiceProvider services, IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            _services = services;
        }

        public IEnumerable<object> Dump()
        {
            //TODO: OPTIONS - Filter on type
            //TODO: OPTIONS - Filter on lifetime

            foreach (var sd in this._serviceCollection)
            {
                var serviceDescriptor = (DiagnosticsServiceDescriptorAttribute) sd.ImplementationType?.GetCustomAttributes(typeof(DiagnosticsServiceDescriptorAttribute), false).FirstOrDefault();

                if (serviceDescriptor != null) yield return new { ServiceType = sd.ServiceType, ImplementationType = sd.ImplementationType, Lifetime = sd.Lifetime.ToString(), Extra = serviceDescriptor.Output(sd, _services) };

                yield return new { ServiceType = sd.ServiceType, ImplementationType = sd.ImplementationType, Lifetime = sd.Lifetime.ToString() };
            }
        }
    }
}