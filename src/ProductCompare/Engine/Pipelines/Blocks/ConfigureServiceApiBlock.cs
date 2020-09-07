using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Conditions;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using System.Diagnostics.Contracts;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Blocks
{
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {
            Contract.Requires(modelBuilder != null);
            Contract.Requires(context != null);

            Condition.Requires(modelBuilder).IsNotNull("The argument can not be null");

            modelBuilder.AddEntityType(typeof(ProductComparison));
            modelBuilder.EntitySet<ProductComparison>("Compare");

            var addProductToComparison = modelBuilder.Action("AddProductToComparison");
            addProductToComparison.Returns<string>();
            addProductToComparison.Parameter<string>("cartId");
            addProductToComparison.Parameter<string>("catalogName");
            addProductToComparison.Parameter<string>("productId");
            addProductToComparison.Parameter<string>("variantId");
            addProductToComparison.ReturnsFromEntitySet<CommerceCommand>("Commands");

            var removeProductFromComparison = modelBuilder.Action("RemoveProductFromComparison");
            removeProductFromComparison.Returns<string>();
            removeProductFromComparison.Parameter<string>("cartId");
            removeProductFromComparison.Parameter<string>("sellableItemId");
            removeProductFromComparison.ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(modelBuilder);
        }
    }
}
