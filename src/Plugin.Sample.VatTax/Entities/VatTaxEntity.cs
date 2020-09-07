namespace Plugin.Bootcamp.Exercises.VatTax.Entities
{
    using System;
    using System.Collections.Generic;

    using Sitecore.Commerce.Core;

    public class VatTaxEntity : CommerceEntity
    {
       // List<Component> Components = null;
        public VatTaxEntity()
        {
            /* STUDENT: Write the body of the default constructor */
           // this.Components = new List<Component>();
            this.DateCreated = DateTime.UtcNow;
            this.DateUpdated = this.DateCreated;
            this.CountryCode = "US";
            this.TaxTag = string.Empty;
            this.TaxPct = 0M;
        }

        public VatTaxEntity(string id) : this()
        {
            /* STUDENT: Write the body of the constructor that is called with the id */
            this.Id = id;
        }

        /* STUDENT: Add read/write properties to the class to meet the requirements */
        public string CountryCode { get; set; }

        public string TaxTag { get; set; }

        public decimal TaxPct { get; set; }

        
    }
}