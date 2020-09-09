using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Framework.Caching.Diagnostics;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CacheDiagnosticsExtensions
    {
        public static ISitecoreServicesConfiguration AddCacheDiagnostics(this ISitecoreServicesConfiguration builder)
        {
            builder.Services.AddSingleton<IDiagnosticPage, CacheDiagnosticsPage>();
            return builder;
        }
    }
}