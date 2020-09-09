using Microsoft.AspNetCore.Http;

namespace Sitecore.Framework.Diagnostics
{
    public interface IPageSecurityProvider
    {
        object Secret { get; }

        bool ValidateSecret(HttpContext context);
    }
}