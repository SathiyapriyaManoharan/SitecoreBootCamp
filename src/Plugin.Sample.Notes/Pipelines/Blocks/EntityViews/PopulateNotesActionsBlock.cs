using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.Notes
{
    [PipelineDisplayName(NotesConstants.PopulateNotesActionsBlock)]
    public class PopulateNotesActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(arg != null);

            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            var viewsPolicy = context.GetPolicy<KnownNotesViewsPolicy>();

            if (string.IsNullOrEmpty(arg?.Name) || 
                !arg.Name.Equals(viewsPolicy.Notes, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            var actionPolicy = arg.GetPolicy<ActionsPolicy>();

            actionPolicy.Actions.Add(
                new EntityActionView
                {
                    Name = context.GetPolicy<KnownNotesActionsPolicy>().EditNotes,
                    DisplayName = "Edit Sellable Item Notes",
                    Description = "Edits the sellable item notes",
                    IsEnabled = true,
                    EntityView = arg.Name,
                    Icon = "edit"
                });

            return Task.FromResult(arg);
        }
    }
}
