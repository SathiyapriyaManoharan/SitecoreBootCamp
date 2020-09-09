using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sitecore.Framework.Conditions;

namespace Sitecore.Framework.Diagnostics
{
    public abstract class SummaryDetailsBaseDiagnosticPage : BaseDiagnosticsPage
    {
        protected SummaryDetailsBaseDiagnosticPage(string defaultPath) : base(defaultPath)
        {
        }

        public abstract object GetSummary();

        public virtual object GetDetails(string id)
        {
            return null;
        }

        public virtual object GetDetails(string id, string subId)
        {
            return null;
        }

        public async Task DisplaySummary(HttpContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var summary = GetSummary();
            await Write(context, summary);
        }

        public async Task DisplayDetails(HttpContext context, string id)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var details = GetDetails(id);
            await Write(context, details);
        }

        public async Task DisplayDetails(HttpContext context, string id, string subId)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var details = GetDetails(id, subId);
            await Write(context, details);
        }

        public override async Task RenderPage(HttpContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();

            var path = context.Request.Path.Value.ToLower().Replace(DefaultPath, string.Empty);

            var tokens = path.Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            switch (tokens.Length)
            {
                case 1:
                    await DisplayDetails(context, tokens[0]);
                    break;

                case 2:
                    await DisplayDetails(context, tokens[0], tokens[1]);
                    break;

                default:
                    await DisplaySummary(context);
                    break;
            }
        }
    }
}