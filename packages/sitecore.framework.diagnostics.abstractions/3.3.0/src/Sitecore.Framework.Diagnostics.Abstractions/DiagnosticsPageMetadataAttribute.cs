using System;
using System.Security.AccessControl;
using System.Threading;

namespace Sitecore.Framework.Diagnostics
{
    public class DiagnosticsPageMetadataAttribute : Attribute
    {
        private readonly string _category;
        private readonly string _helpString;

        public DiagnosticsPageMetadataAttribute(string category, string helpString)
        {
            _category = category;
            _helpString = helpString;
        }

        public DiagnosticsPageMetadata GetMetadata()
        {
            return new DiagnosticsPageMetadata {Category = _category, HelpString = _helpString};
        }
    }
}