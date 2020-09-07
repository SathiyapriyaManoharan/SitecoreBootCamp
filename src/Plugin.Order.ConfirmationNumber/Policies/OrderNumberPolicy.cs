using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies
{
    public class OrderNumberPolicy : Policy
    {
        public OrderNumberPolicy()
        {

            this.Prefix = string.Empty;
            this.Suffix = string.Empty;
            this.IncludedDate = false;

            /* STUDENT: Complete the constructor to initialize the properties */

        }

        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool IncludedDate { get; set; }

    }
    /* STUDENT: Add read/write properties as specified in the requirements */


}
