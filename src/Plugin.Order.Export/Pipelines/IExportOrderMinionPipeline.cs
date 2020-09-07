using XC = Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines
{
    public interface IExportOrderMinionPipeline : IPipeline<ExportOrderArgument, XC.Order, CommercePipelineExecutionContext>
    {
    }
}
