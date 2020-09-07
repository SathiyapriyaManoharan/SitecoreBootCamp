using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("GetVatTaxDashboardViewBlock")]
    
    public class GetVatTaxDashboardViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;
        
        public GetVatTaxDashboardViewBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }
        
        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");
            if (entityView.Name != "VatTaxDashboard")
            {
                return entityView;
            }
            //var pluginPolicy = context.GetPolicy<PluginPolicy>();

            var newEntityViewTable = entityView;
            entityView.UiHint = "Table";
            entityView.Icon = "money_dollar";
            entityView.ItemId = string.Empty;

            var sampleDashboardEntities = await _commerceCommander.Command<ListCommander>()
                .GetListItems<VatTaxEntity>(context.CommerceContext,
                    CommerceEntity.ListName<VatTaxEntity>(), 0, 99).ConfigureAwait(false);

            //var sampleDashboardEntities = await this._commerceCommander.Command<ListCommander>().GetListItems<VatTaxEntity>(context.CommerceContext,CommerceEntity.ListName<VatTaxEntity>(), 0, 99);
            foreach (var sampleDashboardEntity in sampleDashboardEntities)
            {
                newEntityViewTable.ChildViews.Add(
                    new EntityView
                    {
                        ItemId = sampleDashboardEntity.Id,
                        Icon = "money_dollar",
                        Properties = new List<ViewProperty>
                        {
                            new ViewProperty {Name = "TaxTag", RawValue = sampleDashboardEntity.TaxTag },
                            new ViewProperty {Name = "CountryCode", RawValue = sampleDashboardEntity.CountryCode },
                            new ViewProperty {Name = "TaxPct", RawValue = sampleDashboardEntity.TaxPct }
                        }
                    });
            }

            
            /* STUDENT: Complete the body of the Run method. You should handle the 
             * entity view for both a new and existing entity. */
            return entityView;
        }
    }
}
