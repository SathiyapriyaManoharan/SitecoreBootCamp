namespace Plugin.Sample.Order.View.Pipelines
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    using Sitecore.Commerce.EntityViews;

    [PipelineDisplayName("ExportOrderViewPipeline")]
    public interface IOrderViewPipeline : IPipeline<EntityView, EntityView, CommercePipelineExecutionContext>
    {
    }
}
