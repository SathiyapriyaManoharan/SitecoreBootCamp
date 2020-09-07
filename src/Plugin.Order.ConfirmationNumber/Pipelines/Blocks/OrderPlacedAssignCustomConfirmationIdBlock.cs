using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies;
using Sitecore.Commerce.Plugin.Orders;
using System.Diagnostics.Contracts;


namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Blocks
{
    [PipelineDisplayName("OrderConfirmation.OrderConfirmationIdBlock")]
    public class OrderPlacedAssignCustomConfirmationIdBlock : PipelineBlock<Sitecore.Commerce.Plugin.Orders.Order, Sitecore.Commerce.Plugin.Orders.Order, CommercePipelineExecutionContext>
    {
        public override Task<Sitecore.Commerce.Plugin.Orders.Order> Run(Sitecore.Commerce.Plugin.Orders.Order arg, CommercePipelineExecutionContext context)
        {

            Contract.Requires(arg != null);
            Contract.Requires(context != null);
            /* STUDENT: Complete this method to set the order number as specified in the requirements */
            Condition.Requires(arg).IsNotNull($"{this.Name}: The Order cannot be null");
            OrderPlacedAssignCustomConfirmationIdBlock confirmationIdBlock = this;
            string uniqueGuid;
            try
            {
                uniqueGuid = GetCustomOrder(context);
            }
            catch (Exception ex)
            {
                context.CommerceContext.LogException((confirmationIdBlock.Name) + "UniqueCodeException", ex);
                throw;
            }
            arg.OrderConfirmationId = uniqueGuid;
            return Task.FromResult<Sitecore.Commerce.Plugin.Orders.Order>(arg);

        }
        private string GetCustomOrder(CommercePipelineExecutionContext context)
        {
            var policy = context.GetPolicy<OrderNumberPolicy>();
            return policy.IncludedDate == true ? $"{policy.Prefix},{DateTime.Today.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)},{policy.Suffix},{Guid.NewGuid().ToString()}" :
                                                $"{policy.Prefix},{string.Empty},{policy.Suffix},{Guid.NewGuid().ToString()}";
        }
    }
}


