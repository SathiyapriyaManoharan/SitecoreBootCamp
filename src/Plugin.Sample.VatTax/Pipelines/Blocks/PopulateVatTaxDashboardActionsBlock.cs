using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("EnsureActions")]
    public class PopulateVatTaxDashboardActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);

            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            /* STUDENT: Add the necessary code to add the add and remove actions to the table on the Vat Tax dashboard */
            if (entityView.Name != "VatTaxDashboard")
            {
                return Task.FromResult(entityView);
            }

            var tableViewActionsPolicy = entityView.GetPolicy<ActionsPolicy>();
            tableViewActionsPolicy.Actions.Add(new EntityActionView
            {
                Name = "VatTaxDashboard-AddDashboardEntity",
                DisplayName = "Adds a new Vat Tax Entry",
                Description = "Adds a new Vat Tax Entry",
                IsEnabled = true,
                RequiresConfirmation = false,
                EntityView = "VatTaxDashboard-FormAddDashboardEntity",
                Icon = "add"
            });

            tableViewActionsPolicy.Actions.Add(new EntityActionView
            {
                Name = "VatTaxDashboard-RemoveDashboardEntity",
                DisplayName = "Remove Vat Tax Entry",
                Description = "Removes a Vat Tax Entry",
                IsEnabled = true,
                RequiresConfirmation = true,
                EntityView = string.Empty,
                Icon = "delete"
            });


            return Task.FromResult(entityView);
        }
    }
}
