using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Sitecore.Commerce.EntityViews;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines
{
    public class OrderViewPipeline : CommercePipeline<EntityView, EntityView>, IOrderViewPipeline
    {
        public OrderViewPipeline(IPipelineConfiguration<IOrderViewPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

