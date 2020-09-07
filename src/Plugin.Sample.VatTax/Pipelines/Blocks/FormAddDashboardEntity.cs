using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.Pipelines.Blocks
{
    [PipelineDisplayName("FormAddDashboardEntity")]
    public class FormAddDashboardEntity : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            if (entityView.Name != "VatTaxDashboard-FormAddDashboardEntity")
            {
                return Task.FromResult(entityView);
            }

            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = "TaxTag",
                    IsHidden = false,
                    IsRequired = true,
                    RawValue = string.Empty
                });

            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = "CountryCode",
                    IsHidden = false,
                    IsRequired = true,
                    RawValue = string.Empty
                });

            entityView.Properties.Add(
                new ViewProperty
                {
                    Name = "TaxPct",
                    IsHidden = false,
                    IsRequired = true,
                    RawValue = 0
                });

            return Task.FromResult(entityView);
        }


    }

}
