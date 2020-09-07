using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.Export.Policies
{
    public class OrderExportPolicy : Policy
    {
        /*A property for the export file location. Initialize it to default to C:\Temp
         * in case no value is provided by the PolicySet. */
        public string ExportLocation { get; set; }
    }
}
