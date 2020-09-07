using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines
{
    public class GetProductComparePipeline : CommercePipeline<string, ProductComparison>, IGetProductComparePipeline
    {
        public GetProductComparePipeline(IPipelineConfiguration<IGetProductComparePipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
