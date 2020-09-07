using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using XC = Sitecore.Commerce.Plugin.Orders;


namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines
{
    public class ExportOrderMinionPipeline : CommercePipeline<ExportOrderArgument, XC.Order>, IExportOrderMinionPipeline, IPipeline<ExportOrderArgument, XC.Order, CommercePipelineExecutionContext>
    {
        public ExportOrderMinionPipeline(IPipelineConfiguration<IExportOrderMinionPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}