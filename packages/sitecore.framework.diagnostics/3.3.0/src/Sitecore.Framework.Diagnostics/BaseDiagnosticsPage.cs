using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Diagnostics
{
    public abstract class BaseDiagnosticsPage : IDiagnosticPage
    {
        public string DefaultPath { get;}

        public IPageOutputSerializer PageOutputSerializer { get; set; }

        protected BaseDiagnosticsPage(string defaultPath)
        {
            Condition.Requires(defaultPath, nameof(defaultPath)).IsNotNullOrEmpty();
            DefaultPath = defaultPath.ToLowerInvariant();
        }

        public abstract Task RenderPage(HttpContext context);

        protected async Task Write(HttpContext context, object value)
        {
            if (value == null)
            {
                context.Response.StatusCode = 404;
                return;
            }
            context.Response.ContentType = PageOutputSerializer.ContentType;
            var serializedValue = PageOutputSerializer.Serialize(value);
            await context.Response.WriteAsync(serializedValue);
        }
    }
}