using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sitecore.Framework.Diagnostics
{
    public interface IDiagnosticPage
    {
        string DefaultPath { get; }

        IPageOutputSerializer PageOutputSerializer { get; set; }

        Task RenderPage(HttpContext context);
    }
}