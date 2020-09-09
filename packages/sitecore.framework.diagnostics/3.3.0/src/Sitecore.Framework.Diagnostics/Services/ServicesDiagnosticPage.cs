using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sitecore.Framework.Diagnostics.Services
{
    [DiagnosticsPageMetadata("Services Diagnostics", "Use this page to view information about the registered services")]

    public class ServicesDiagnosticPage : BaseDiagnosticsPage
    {
        private readonly ISitecoreServiceDiagnostics _diagnostics;

        public ServicesDiagnosticPage(ISitecoreServiceDiagnostics diagnostics) : base("/admin/services")
        {
            _diagnostics = diagnostics;
        }

        public override Task RenderPage(HttpContext context)
        {
            return Write(context, _diagnostics.Dump());
        }
    }
}