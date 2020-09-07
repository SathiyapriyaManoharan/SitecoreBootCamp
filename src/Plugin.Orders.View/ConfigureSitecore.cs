using Plugin.Sample.Order.View.Pipelines.Blocks;

namespace Plugin.Sample.Order.View
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Orders;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

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

            services.Sitecore().Pipelines(config => config

             .ConfigurePipeline<IGetEntityViewPipeline>(
                    configure =>
                        {
                            configure.Add<OrdersEntityViewBlock>().After<GetOrderLinesViewBlock>();
                        })

            .ConfigurePipeline<IGetEntityViewPipeline>(configure =>
            {
                configure.Add<OrdersEntityViewBlock>().After<GetOrderLinesViewBlock>();
            }));



      

            services.RegisterAllCommands(assembly);
        }
    }
}