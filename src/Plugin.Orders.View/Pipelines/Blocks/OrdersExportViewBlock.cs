using Plugin.Sample.Order.Export.Components;

namespace Plugin.Sample.Order.View.Pipelines.Blocks
{
    using System;
    using System.Threading.Tasks;

    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using Sitecore.Commerce.EntityViews;
    using System.Linq;
    using Sitecore.Commerce.Plugin.Orders;

   [PipelineDisplayName("OrdersEntityViewBlock")]
    public class OrdersEntityViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public OrdersEntityViewBlock(ViewCommander viewCommander)
        {
            this._viewCommander = viewCommander;
        }

        public override  Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            var request = this._viewCommander.CurrentEntityViewArgument(context.CommerceContext);
            if (request.ViewName != context.GetPolicy<KnownOrderViewsPolicy>().Summary
                 && request.ViewName != context.GetPolicy<KnownOrderViewsPolicy>().Master)
            {
                // Do nothing if this request is for a different view
                return Task.FromResult(entityView);
            }
            if (request.Entity == null)
            {
                // Do nothing if there is no entity loaded
                return Task.FromResult(entityView);
            }
            // Only do something if the Entity is an order
            if (!(request.Entity is Order))
            {
                return Task.FromResult(entityView);
            }
          
            //   EntityView entityViewToProcess;
               var order = (Order)request.Entity;
               var component = order.GetComponent<ExportedOrderComponent>();
               if (!component.DateExported.HasValue)
               {
                   return Task.FromResult(entityView);
               }

               var view = new EntityView   //Create a top level view
               {
                   Name = "Is order Exported",
                   DisplayName = "Export Details",
                   EntityId = entityView.EntityId,

               };
            entityView.ChildViews.Add(view); //Add child views to it

            //Add properties and values to the view
            view.Properties.Add(
                      new ViewProperty
                      {
                          Name = "Exported Location",
                          RawValue = component.ExportFilename,
                          IsReadOnly = true,
                          IsRequired = false
                      });
            view.Properties.Add(
                new ViewProperty
                {
                    Name = "Exported Date",
                    RawValue = component.DateExported,
                    IsReadOnly = true,
                    IsRequired = false
                });
            return Task.FromResult(entityView);
        }
    }
    }
