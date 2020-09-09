namespace Sitecore.Framework.Diagnostics
{
    public class DiagnosticsMetadataOptions
    {
        public string Url { get; set; }
        public IPageOutputSerializer PageOutputSerializer { get; set; } = new JsonPageOutputSerializer();
    }
}