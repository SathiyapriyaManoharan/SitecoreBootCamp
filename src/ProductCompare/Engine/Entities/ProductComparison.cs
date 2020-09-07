using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Collections.Generic;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Entities
{
    public class ProductComparison : CommerceEntity
    {
        [Contained]
        public IEnumerable<SellableItem> Products { get; set; }
    }
}
