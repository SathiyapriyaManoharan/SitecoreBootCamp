using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks
{
    [PipelineDisplayName("GetWarrantyNotesViewBlock")]
    public class GetWarrantyNotesViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            /*Various Views */

            var isVariationView = arg.Name.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
            var isDetailView = arg.Name.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase);
            var isWarrantyNotesView = arg.Name.Equals("Warranty Notes", StringComparison.OrdinalIgnoreCase);
            var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);
            var request = context.CommerceContext.GetObject<EntityViewArgument>();


            if (string.IsNullOrEmpty(arg.Name) || !isDetailView && !isWarrantyNotesView && !isVariationView && !isConnectView)
            {
                return Task.FromResult(arg);
            }

            if (!(request.Entity is SellableItem))
            {
                return Task.FromResult(arg);
            }

            /*Check for Sellable Item*/
            var sellableItem = (SellableItem)request.Entity;

            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
            }


            var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals("WarrantyNotes-Edit", StringComparison.OrdinalIgnoreCase);

            /*Check if the Warranty Notes EntityView is in Edit Mode*/

            var componentView = arg;

            /*If Warranty Notes is not in Edit View*/
            if (!isEditView)
            {
                componentView = new EntityView
                {
                    Name = "Warranty Notes",
                    DisplayName = "Warranty Information",
                    EntityId = arg.EntityId,
                    EntityVersion = request.EntityVersion == null ? 1 : (int)request.EntityVersion,
                    ItemId = variationId
                };

                arg.ChildViews.Add(componentView);
                System.Diagnostics.Debug.WriteLine($"Get Entity ViewBlock in Master View. Version from argument is {arg.EntityId}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Get Entity ViewBlock in edit View. Version from argument is {arg.EntityId}");
            }

            /*If Warranty Notes Component is in Edit View*/

            if (sellableItem != null && (sellableItem.HasComponent<WarrantyNotesComponent>(variationId) || isConnectView || isEditView))
            {
                var component = sellableItem.GetComponent<WarrantyNotesComponent>(variationId);

                componentView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(WarrantyNotesComponent.WarrantyInformation),
                    DisplayName = "Description",
                    RawValue = component.WarrantyInformation,
                    IsReadOnly = !isEditView,
                    IsRequired = false
                });

                componentView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(WarrantyNotesComponent.NoOfYears),
                    DisplayName = "Warranty Terms (Years)",
                    RawValue = component.NoOfYears,
                    IsReadOnly = !isEditView,
                    IsRequired = false
                });

                System.Diagnostics.Debug.WriteLine($"Description is {component.WarrantyInformation},term is {component.NoOfYears}");
            }
            return Task.FromResult(arg);
        }

    }
}
