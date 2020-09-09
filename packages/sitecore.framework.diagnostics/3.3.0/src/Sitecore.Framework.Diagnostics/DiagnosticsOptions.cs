namespace Sitecore.Framework.Diagnostics
{
    public class DiagnosticsOptions
    {
        public IPageSecurityProvider PageSecurityProvider { get; set; }
        
        public IPageOutputSerializer PageOutputSerializer { get; set; } = new JsonPageOutputSerializer();
    }
}