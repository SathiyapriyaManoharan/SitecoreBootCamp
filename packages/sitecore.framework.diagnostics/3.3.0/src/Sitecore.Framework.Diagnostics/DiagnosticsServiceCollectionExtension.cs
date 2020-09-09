using Microsoft.Extensions.DependencyInjection;
using Sitecore.Framework.Diagnostics;
using Sitecore.Framework.Diagnostics.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DiagnosticsServiceCollectionExtension
    {
        public static IServiceCollection AddSitecoreServicesDiagnosticsPage(this IServiceCollection services)
        {
            services.AddSingleton<ISitecoreServiceDiagnostics>(sp => ActivatorUtilities.CreateInstance<SitecoreServiceDiagnostics>(sp, services));
            services.AddSingleton<IDiagnosticPage, ServicesDiagnosticPage>();
            return services;
        }
    }
}