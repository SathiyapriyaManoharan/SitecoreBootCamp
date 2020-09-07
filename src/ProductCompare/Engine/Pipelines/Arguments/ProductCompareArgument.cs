using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Arguments
{
    public class ProductCompareArgument : PipelineArgument
    {
        public ProductComparison CompareCollection { get; }

        protected ProductCompareArgument(ProductComparison compareCollection)
        {
            CompareCollection = compareCollection;
        }
    }
}
