using Plugin.Bootcamp.Exercises.ProductCompare.Entities;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Arguments
{
    public class AddToProductCompareArgument : ProductCompareArgument
    {
        public string CatalogName { get; }
        public string ProductId { get; }
        public string VariantId { get; }

        public AddToProductCompareArgument(ProductComparison compareCollection, string catalogName, string productId, string variantId) : base(compareCollection)
        {
            CatalogName = catalogName;
            ProductId = productId;
            VariantId = variantId;
        }
    }
}
