using Microsoft.Extensions.DependencyInjection;
using Plugin.Bootcamp.Exercises.VatTax.EntityViews;
using Plugin.Bootcamp.Exercises.VatTax.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.BusinessUsers;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using System.Reflection;

namespace Plugin.Bootcamp.Exercises.VatTax
{
    public class ConfigureSitecore : IConfigureSitecore
    {
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                  /* STUDENT: Add code here to configure the necessary pipelines to show your navigation, present your
                   * dashboard, present your add form, and handle your actions. */
                  .ConfigurePipeline<IBizFxNavigationPipeline>(d =>
                  {
                      d.Add<GetVatTaxNavigationViewBlock>();
                  })

               
               .ConfigurePipeline<IGetEntityViewPipeline>(d =>
                {
                    d.Add<GetVatTaxDashboardViewBlock>().Before<IFormatEntityViewPipeline>()
                        .Add<FormAddDashboardEntity>().Before<IFormatEntityViewPipeline>();
                })
                .ConfigurePipeline<IFormatEntityViewPipeline>(d =>
                {
                    d.Add<PopulateVatTaxDashboardActionsBlock>().After<PopulateEntityViewActionsBlock>();
                })
                .ConfigurePipeline<IDoActionPipeline>(
                    c =>
                    {
                        c.Add<DoActionAddVatTaxBlock>().After<ValidateEntityVersionBlock>()
                            .Add<DoActionRemoveVatTaxBlock>().After<ValidateEntityVersionBlock>();
                    })
                    .ConfigurePipeline<ICalculateCartLinesPipeline>(d =>
                    {
                        d.Replace<Sitecore.Commerce.Plugin.Tax.CalculateCartLinesTaxBlock, CalculateCartLinesTaxBlockCustom>();
                    })
                    );


            services.RegisterAllCommands(assembly);
        }
    }
}