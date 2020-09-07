using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Sample.USPS.Policies
{
    public class USPSPolicy : Policy
    {
        public USPSPolicy()
        {
            this.DebugMode = true;
            this.UserId = string.Empty;
            this.Url = string.Empty;
        }
        public bool DebugMode { get; set; }
        public string UserId { get; set; }
#pragma warning disable CA1056 // Uri properties should not be strings
        public string Url { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
    }
}
