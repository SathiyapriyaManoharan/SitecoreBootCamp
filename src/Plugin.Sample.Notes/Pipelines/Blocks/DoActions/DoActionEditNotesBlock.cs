using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.Notes
{
    [PipelineDisplayName(NotesConstants.DoActionEditNotesBlock)]
    public class DoActionEditNotesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionEditNotesBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(arg != null);
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var notesActionsPolicy = context.GetPolicy<KnownNotesActionsPolicy>();

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(notesActionsPolicy.EditNotes, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            // Get the sellable item from the context
            var entity = context.CommerceContext.GetObject<SellableItem>(x => x.Id.Equals(arg.EntityId, StringComparison.InvariantCultureIgnoreCase));
            if (entity == null)
            {
                return Task.FromResult(arg);
            }

            // Get the notes component from the sellable item or its variation
            var component = entity.GetComponent<NotesComponents>(arg.ItemId);

            // Map entity view properties to component
            component.WarrantyInformation =
                arg.Properties.FirstOrDefault(x =>
                        x.Name.Equals(nameof(NotesComponents.WarrantyInformation), StringComparison.OrdinalIgnoreCase))?.Value;

            component.InternalNotes =
                arg.Properties.FirstOrDefault(x =>
                    x.Name.Equals(nameof(NotesComponents.InternalNotes), StringComparison.OrdinalIgnoreCase))?.Value;

            // Persist changes
            this._commerceCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
