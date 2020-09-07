using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Framework.Pipelines;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("DoActionAddDashboardEntity")]
    public class DoActionAddVatTaxBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionAddVatTaxBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);

            /* STUDENT: Complete this method by adding the code to save a new Vat Tax configuration item */
            if (entityView == null
                || !entityView.Action.Equals("VatTaxDashboard-AddDashboardEntity", StringComparison.OrdinalIgnoreCase))
            {
                return entityView;
            }

            try
            {
                var taxTag = entityView.Properties.First(p => p.Name == "TaxTag").Value ?? "";
                var countryCode = entityView.Properties.First(p => p.Name == "CountryCode").Value ?? "";
                var taxPct = System.Convert.ToDecimal(entityView.Properties.First(p => p.Name == "TaxPct").Value ?? "0");

                var sampleDashboardEntity = new VatTaxEntity
                {
                    Id = CommerceEntity.IdPrefix<VatTaxEntity>() + Guid.NewGuid().ToString("N"),
                    Name = string.Empty,
                    TaxTag = taxTag,
                    CountryCode = countryCode,
                    TaxPct = taxPct
                };

                sampleDashboardEntity.GetComponent<ListMembershipsComponent>().Memberships.Add(CommerceEntity.ListName<VatTaxEntity>());

                await this._commerceCommander.PersistEntity(context.CommerceContext, sampleDashboardEntity);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Catalog.DoActionAddDashboardEntity.Exception: Message={ex.Message}");
            }


            return entityView;
        }
    }
}
