// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation
{
    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Pipelines(config => config

                .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                {
                    c.Add<GetWarrantyNotesViewBlock>().After<GetSellableItemDetailsViewBlock>();
                })
                .ConfigurePipeline<IPopulateEntityViewActionsPipeline>(c =>
                 {
                     c.Add<PopulateWarrantyNotesActionsBlock>().After<InitializeEntityViewActionsBlock>();
                 })
                .ConfigurePipeline<IDoActionPipeline>(c =>
                {
                    c.Add<DoActionEditWarrantyNotesBlock>().After<ValidateEntityVersionBlock>();
                })
            );
        }
    }
}
