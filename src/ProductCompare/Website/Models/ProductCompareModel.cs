using Sitecore.Commerce.XA.Foundation.Common.Models;
using System.Collections.Generic;


namespace Feature.Website.Models
{
    public class ProductCompareModel : BaseCommerceRenderingModel
    {
        public IList<ProductCompareListItemModel> Products { get; set; }
        public bool IsValid { get; set; }
        public string RemoveFromCompareText { get; set; }        
    }
}