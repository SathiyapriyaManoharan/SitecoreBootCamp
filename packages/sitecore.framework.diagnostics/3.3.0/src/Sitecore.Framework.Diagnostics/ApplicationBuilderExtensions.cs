using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sitecore.Framework.Diagnostics
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDiagnostics(this IApplicationBuilder builder)
        {
            return UseDiagnostics(builder, new DiagnosticsOptions(), new DiagnosticsMetadataOptions() {Url = "/admin/metadata"});
        }

        public static IApplicationBuilder UseDiagnostics(this IApplicationBuilder builder, DiagnosticsOptions options, DiagnosticsMetadataOptions metadataOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder
                .Use(next => new DiagnosticPageMiddleware(next, builder.ApplicationServices.GetServices<IDiagnosticPage>(), options).Invoke)
                .Use(next => new DiagnosticMetadataMiddleware(next, builder.ApplicationServices.GetServices<IDiagnosticPage>(), metadataOptions).Invoke);
        }
    }
}