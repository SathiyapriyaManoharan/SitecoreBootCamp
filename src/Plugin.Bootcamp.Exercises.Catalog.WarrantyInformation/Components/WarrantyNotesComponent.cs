using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components
{
    public class WarrantyNotesComponent : Component
    {
        /* STUDENT: Add properties as specified in the requirements */
   
        public string WarrantyInformation { get; set; } = string.Empty;
        public int NoOfYears { get; set; } = 1;
    }
}
