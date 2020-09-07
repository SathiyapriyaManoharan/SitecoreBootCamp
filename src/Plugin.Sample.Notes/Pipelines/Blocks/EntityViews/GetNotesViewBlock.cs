using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.Notes
{
    [PipelineDisplayName(NotesConstants.GetNotesViewBlock)]
    public class GetNotesViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetNotesViewBlock(ViewCommander viewCommander)
        {
            this._viewCommander = viewCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(arg != null);

            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var request = this._viewCommander.CurrentEntityViewArgument(context.CommerceContext);

            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            var notesViewsPolicy = context.GetPolicy<KnownNotesViewsPolicy>();
            var notesActionsPolicy = context.GetPolicy<KnownNotesActionsPolicy>();

            var isVariationView = request.ViewName.Equals(catalogViewsPolicy.Variant, StringComparison.OrdinalIgnoreCase);
            var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.OrdinalIgnoreCase);

            // Make sure that we target the correct views
            if (string.IsNullOrEmpty(request.ViewName) ||
                !request.ViewName.Equals(catalogViewsPolicy.Master, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(catalogViewsPolicy.Details, StringComparison.OrdinalIgnoreCase) &&
                !request.ViewName.Equals(notesViewsPolicy.Notes, StringComparison.OrdinalIgnoreCase) &&
                !isVariationView &&
                !isConnectView)
            {
                return Task.FromResult(arg);
            }

            // Only proceed if the current entity is a sellable item
            if (!(request.Entity is SellableItem))
            {
                return Task.FromResult(arg);
            }

            var sellableItem = (SellableItem) request.Entity;

            // See if we are dealing with the base sellable item or one of its variations.
            var variationId = string.Empty;
            if (isVariationView && !string.IsNullOrEmpty(arg.ItemId))
            {
                variationId = arg.ItemId;
            }
            
            var targetView = arg;

            // Check if the edit action was requested
            var isEditView = !string.IsNullOrEmpty(arg.Action) && arg.Action.Equals(notesActionsPolicy.EditNotes, StringComparison.OrdinalIgnoreCase);
            if (!isEditView)
            {
                // Create a new view and add it to the current entity view.
                var view = new EntityView
                {
                    Name = context.GetPolicy<KnownNotesViewsPolicy>().Notes,
                    DisplayName = "Notes",
                    EntityId = arg.EntityId,
                    ItemId = variationId
                };

                arg.ChildViews.Add(view);

                targetView = view;
            }

            if (sellableItem != null && (sellableItem.HasComponent<NotesComponents>(variationId) || isConnectView || isEditView))
            {
                var component = sellableItem.GetComponent<NotesComponents>(variationId);
                AddPropertiesToView(targetView, component, !isEditView);
            }

            return Task.FromResult(arg);
        }

        private void AddPropertiesToView(EntityView entityView, NotesComponents component, bool isReadOnly)
        {
            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(NotesComponents.WarrantyInformation),
                    RawValue = component.WarrantyInformation,
                    IsReadOnly = isReadOnly,
                    IsRequired = false,
                    OriginalType = "Html"
                });

            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = nameof(NotesComponents.InternalNotes),
                    RawValue = component.InternalNotes,
                    IsReadOnly = isReadOnly,
                    IsRequired = false
                });
        }
    }
}
