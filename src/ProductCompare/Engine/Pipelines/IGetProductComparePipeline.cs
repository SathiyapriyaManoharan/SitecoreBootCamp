using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines
{
    public interface IGetProductComparePipeline : IPipeline<string, ProductComparison, CommercePipelineExecutionContext>
    {
    }
}
