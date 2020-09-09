using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components
{
    public class WarrantyNotesComponent : Component
    {
        /* Warranty Notes Component with two properties */

        public string WarrantyInformation { get; set; } = string.Empty;
        public int NoOfYears { get; set; } = 1;
    }
}
