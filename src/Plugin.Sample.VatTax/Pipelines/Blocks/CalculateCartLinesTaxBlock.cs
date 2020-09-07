using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Commerce.Plugin.Tax;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.VatTax.Pipelines.Blocks
{
    [PipelineDisplayName("CalculateCartLinesTax")]
    public class CalculateCartLinesTaxBlockCustom : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public CalculateCartLinesTaxBlockCustom(CommerceCommander commander)
        {
            this._commerceCommander = commander;
        }

        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(arg != null);
            Contract.Requires(context != null);

            Condition.Requires(arg).IsNotNull($"{this.Name}: The cart can not be null");
            Condition.Requires(arg.Lines).IsNotNull($"{this.Name}: The cart lines can not be null");

            if (!arg.Lines.Any())
            {
                return arg;
            }

            var currency = context.CommerceContext.CurrentCurrency();

            var globalTaxPolicy = context.GetPolicy<GlobalTaxPolicy>();
            var globalPricingPolicy = context.GetPolicy<GlobalPricingPolicy>();
            context.Logger.LogDebug($"{this.Name} - Policy:{globalTaxPolicy.TaxCalculationEnabled}");

            /* STUDENT: Uncomment lines 47-98 after you have implemented the VatTax entity */
            
            var vatTaxTable = await this._commerceCommander.Command<ListCommander>()
                .GetListItems<VatTaxEntity>(context.CommerceContext,
                    CommerceEntity.ListName<VatTaxEntity>(), 0, 99).ConfigureAwait(false);

            foreach (var line in arg.Lines)
            {
                var lineProductComponent = line.GetComponent<CartProductComponent>();
                var tags = lineProductComponent.Tags;
                
                foreach (var taxRateLine in vatTaxTable)
                {
                    if (tags.Any(p => p.Name == taxRateLine.TaxTag))
                    {
                        if (arg.HasComponent<PhysicalFulfillmentComponent>())
                        {
                            var physicalFulfillment = arg.GetComponent<PhysicalFulfillmentComponent>();
                            var party = physicalFulfillment.ShippingParty;
                            var country = party.CountryCode;

                            if (taxRateLine.CountryCode == country)
                            {
                                var taxRate = taxRateLine.TaxPct / 100;
                                context.Logger.LogDebug($"{this.Name} - Item Tax Rate:{taxRate}");
                                var adjustmentTotal = line.Adjustments.Where(a => a.IsTaxable).Aggregate(0M, (current, adjustment) => current + adjustment.Adjustment.Amount);
                                context.Logger.LogDebug($"{this.Name} - SubTotal:{line.Totals.SubTotal.Amount}");
                                context.Logger.LogDebug($"{this.Name} - Adjustment Total:{adjustmentTotal}");
                                var tax = new Money(currency, (line.Totals.SubTotal.Amount + adjustmentTotal) * taxRate);

                                if (globalPricingPolicy.ShouldRoundPriceCalc)
                                {
                                    tax.Amount = decimal.Round(
                                        tax.Amount,
                                        globalPricingPolicy.RoundDigits,
                                        globalPricingPolicy.MidPointRoundUp ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven);
                                }
                              
                                line.Adjustments.Add(new CartLineLevelAwardedAdjustment
                                {
                                    Name = TaxConstants.TaxAdjustmentName,
                                   
                                    DisplayName = $"{taxRate * 100}% Vat Tax",
                                    Adjustment = tax,
                                    AdjustmentType = context.GetPolicy<KnownCartAdjustmentTypesPolicy>().Tax,
                                    AwardingBlock = this.Name,
                                    IsTaxable = false,
                                    IncludeInGrandTotal = true
                                });
                            }
                        }
                    }
                } 
        }
            return arg;
        }
    }
}
