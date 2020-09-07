using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Sitecore.Commerce.EntityViews;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines
{
    [PipelineDisplayName("ExportOrderViewPipeline")]
    public interface IOrderViewPipeline : IPipeline<EntityView, EntityView, CommercePipelineExecutionContext>
    {
    }
}
