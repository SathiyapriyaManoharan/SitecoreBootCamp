using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;

namespace Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments
{
    public class ExportOrderArgument : PipelineArgument
    {
        public string OrderId { get; set; }

        public ExportOrderArgument(string orderId)
        {
            /* check that a valid orderId */
            Condition.Requires<string>(orderId, "orderId").IsNotNullOrEmpty();

            this.OrderId = orderId;

        }
    }
}