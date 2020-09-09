using System.Collections.Generic;

namespace Sitecore.Framework.Diagnostics.Services
{
    public interface ISitecoreServiceDiagnostics
    {
        IEnumerable<object> Dump();
    }
}