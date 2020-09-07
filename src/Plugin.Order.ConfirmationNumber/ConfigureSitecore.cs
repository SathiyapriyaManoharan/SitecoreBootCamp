using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Commerce.Plugin.Orders;
using Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Blocks;

namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber
{    
    public class ConfigureSitecore : IConfigureSitecore
    {
#pragma warning disable CA1822 // Mark members as static
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1822 // Mark members as static
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            /* STUDENT: Add the necessary method to configure the appropriate pipeline
             * to fulfill the specified requirements */
            services.Sitecore().Pipelines(config => config

               .ConfigurePipeline<IOrderPlacedPipeline>(configure =>
               {
                   configure.Replace<OrderPlacedAssignConfirmationIdBlock, OrderPlacedAssignCustomConfirmationIdBlock>();
               })

           );

            services.RegisterAllCommands(assembly);
        }
    }
}