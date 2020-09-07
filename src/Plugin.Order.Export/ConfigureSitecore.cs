using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Blocks;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Bootcamp.Exercises.Order.Export
{
    public class ConfigureSitecore : IConfigureSitecore
    {
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            /* STUDENT: Uncomment lines 29-36 to enable an entity view
            *  that will allow you to see the status of the export when
            *  viewing an order in the business tools.
            *  
            *  You will also need to add code to add your new pipeline
            *  and add blocks to it. */
            
            services.Sitecore().Pipelines(config => config
            
              .ConfigurePipeline<IGetEntityViewPipeline>(configure =>
              {
                  configure.Add<OrdersEntityViewBlock>().After<GetOrderLinesViewBlock>();
              })
           
             );
             

            services.Sitecore().Pipelines(config => config
             .AddPipeline<IExportOrderMinionPipeline, ExportOrderMinionPipeline>(
                    configure =>
                    {
                        configure.Add<RetrieveOrderBlock>().Add<ExportOrderToFileBlock>();
                    })
            );

            services.RegisterAllCommands(assembly);

        }
    }
}
