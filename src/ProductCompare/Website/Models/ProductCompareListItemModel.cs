using Sitecore.Commerce.XA.Feature.Catalog.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models;

namespace Feature.Website.Models
{
    public class ProductCompareListItemModel : BaseCommerceRenderingModel
    {
        public string  SellableItemId { get; set; }
        public int Depth { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public CatalogItemRenderingModel CatalogItemRenderingModel { get; set; }
    }
}