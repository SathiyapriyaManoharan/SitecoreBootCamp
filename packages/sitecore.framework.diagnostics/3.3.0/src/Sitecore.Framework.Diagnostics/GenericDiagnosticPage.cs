using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Diagnostics
{
    public abstract class GenericDiagnosticPage : BaseDiagnosticsPage
    {
        protected GenericDiagnosticPage(string defaultPath) : base(defaultPath)
        {
        }

        public abstract object GetSummary();

        public virtual object GetDetails(string category, string[] ids)
        {
            return null;
        }

        public virtual async Task DisplaySummary(HttpContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var summary = GetSummary();
            await Write(context, summary);
        }

        public virtual async Task DisplayDetails(HttpContext context, string category, string[] ids)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var details = GetDetails(category, ids);
            await Write(context, details);
        }

        public override async Task RenderPage(HttpContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var path = context.Request.Path.Value.ToLower().Replace(DefaultPath, string.Empty);

            var tokens = path.Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            switch (tokens.Length)
            {
                case 0:
                    await DisplaySummary(context);
                    break;

                default:
                    await DisplayDetails(context, tokens[0], tokens.Skip(1).ToArray());
                    break;

                    
            }
        }
    }
}