using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Diagnostics
{
    public class DiagnosticMetadataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<IDiagnosticPage> _diagnosticPages;
        private readonly string _url;
        private readonly IPageOutputSerializer _pageSerializer;

        public DiagnosticMetadataMiddleware(RequestDelegate next, IEnumerable<IDiagnosticPage> diagnosticPages, DiagnosticsMetadataOptions options)
        {
            _next = next;
            _diagnosticPages = diagnosticPages;
            _url = options.Url;
            _pageSerializer = options.PageOutputSerializer;
        }

        public virtual async Task Invoke(HttpContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            if (context.Request.Path.Equals(_url))
            {
                var pagesMetadata = _diagnosticPages.Select(x =>
                {
                    var metadata = GetPageMetadata(x);
                    return new {ServiceUrl = x.DefaultPath, metadata?.Category, metadata?.HelpString};
                });

                context.Response.ContentType = _pageSerializer.ContentType;
                context.Response.Headers.Append("Access-Control-Allow-Origin","*");
                var serializedValue = _pageSerializer.Serialize(pagesMetadata);
                await context.Response.WriteAsync(serializedValue);
                return;
            }

            await _next(context);
        }

        protected virtual DiagnosticsPageMetadata GetPageMetadata(IDiagnosticPage page)
        {
            var attribute = page.GetType().GetCustomAttribute<DiagnosticsPageMetadataAttribute>(false);
            return attribute?.GetMetadata();
        }

    }
}