using Microsoft.AspNetCore.Http;

namespace Sitecore.Framework.Diagnostics
{
    public class QueryStringPageSecurityProvider : IPageSecurityProvider
    {
        public object Secret { get; }

        public string SecretKey { get; }

        public QueryStringPageSecurityProvider(string secretKey, string secret)
        {
            SecretKey = secretKey.ToLowerInvariant();
            Secret = secret.ToLowerInvariant();
        }

        public bool ValidateSecret(HttpContext context)
        {
            var qs = context.Request.Query;

            return qs.ContainsKey(SecretKey) && qs[SecretKey] == Secret.ToString();
        }
    }
}