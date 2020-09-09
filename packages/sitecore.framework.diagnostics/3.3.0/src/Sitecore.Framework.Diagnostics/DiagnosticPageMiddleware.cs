using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Diagnostics
{
    public class DiagnosticPageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<IDiagnosticPage> _diagnosticPages;
        private readonly DiagnosticsOptions _options;

        public DiagnosticPageMiddleware(RequestDelegate next, IEnumerable<IDiagnosticPage> diagnosticPages, DiagnosticsOptions options)
        {
            _next = next;
            _diagnosticPages = diagnosticPages;
            _options = options;
        }

        public virtual async Task Invoke(HttpContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            foreach (var diagnosticPage in _diagnosticPages)
            {
                if (context.Request.Path.StartsWithSegments(diagnosticPage.DefaultPath))
                {
                    if (_options.PageSecurityProvider != null && !_options.PageSecurityProvider.ValidateSecret(context))
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }

                    diagnosticPage.PageOutputSerializer = _options.PageOutputSerializer;
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");

                    await diagnosticPage.RenderPage(context);

                    return;
                }
            }

            await _next(context);
        }
    }
}