using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Arguments;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines
{
    public class AddToProductComparePipeline : CommercePipeline<AddToProductCompareArgument, ProductComparison>, IAddToProductComparePipeline
    {
        public AddToProductComparePipeline(IPipelineConfiguration<IAddToProductComparePipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
