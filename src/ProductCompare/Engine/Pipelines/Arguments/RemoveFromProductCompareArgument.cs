using Plugin.Bootcamp.Exercises.ProductCompare.Entities;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Arguments
{
    public class RemoveFromProductCompareArgument : ProductCompareArgument
    {
        public string SellableItemId { get; }

        public RemoveFromProductCompareArgument(ProductComparison compareCollection, string sellableItemId) : base(compareCollection)
        {
            SellableItemId = sellableItemId;
        }
    }
}
