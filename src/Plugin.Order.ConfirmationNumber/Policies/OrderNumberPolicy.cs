using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies
{
    public class OrderNumberPolicy : Policy
    {
        public OrderNumberPolicy()
        {
            /* Properties for the Order Confirmation Number*/

            this.Prefix = string.Empty;
            this.Suffix = string.Empty;
            this.IncludedDate = false;

        }

        /* Read/Write Properties for the Order Confirmation Number */
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool IncludedDate { get; set; }

    }

}
