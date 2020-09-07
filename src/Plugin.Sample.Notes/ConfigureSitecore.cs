using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Bootcamp.Exercises.Notes
{
    public class ConfigureSitecore : IConfigureSitecore
    {
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config =>
                config
                    .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                    {
                        c.Add<GetNotesViewBlock>().After<GetSellableItemDetailsViewBlock>();
                    })
                    .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                    {
                        c.Add<PopulateNotesActionsBlock>().After<InitializeEntityViewActionsBlock>();
                    })
                    .ConfigurePipeline<IDoActionPipeline>(c =>
                    {
                        c.Add<DoActionEditNotesBlock>().After<ValidateEntityVersionBlock>();
                    })
            );
        }
    }
}
