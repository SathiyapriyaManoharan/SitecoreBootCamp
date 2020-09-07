using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Core;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Plugin.Bootcamp.Exercises.ProductCompare.Pipelines;
using Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Arguments;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Commands
{
    public class AddToProductCompareCommand : CommerceCommand
    {
        private readonly IAddToProductComparePipeline _addToProductComparePipeline;

        public AddToProductCompareCommand(IAddToProductComparePipeline addToProductComparePipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._addToProductComparePipeline = addToProductComparePipeline; // Added 
        }

        public virtual async Task<ProductComparison> Process(CommerceContext commerceContext, string cartId, string catalogName, string productId, string variantId)
        {
            using (CommandActivity.Start(commerceContext, this))
            {
                var getProductCompareCommand = Command<GetProductCompareCommand>();
                var productComparison = await getProductCompareCommand.Process(commerceContext, cartId).ConfigureAwait(false);

                var arg = new AddToProductCompareArgument(productComparison, catalogName, productId, variantId);
                return await _addToProductComparePipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext)).ConfigureAwait(false);
            }
        }
    }
}
