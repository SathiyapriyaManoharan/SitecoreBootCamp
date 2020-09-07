using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Sitecore.Framework.Conditions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sitecore.Commerce.Plugin.Catalog;
using System.Linq;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Blocks
{
    public class GetProductCompareBlock : PipelineBlock<string, ProductComparison, CommercePipelineExecutionContext>
    {
        private readonly IFindEntitiesInListPipeline _findEntitiesInListPipeline;

        public GetProductCompareBlock(IFindEntitiesInListPipeline findEntitiesInListPipeline)
        {
            _findEntitiesInListPipeline = findEntitiesInListPipeline;
        }

        public override async Task<ProductComparison> Run(string arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The compare collection id can not be null");

            var productComparison = new ProductComparison
            {
                Name = arg,
                Products = await GetListItems(arg, 10, context).ConfigureAwait(false)
            };
            return productComparison;
        }

        protected virtual async Task<IEnumerable<SellableItem>> GetListItems(string listName, int take, CommercePipelineExecutionContext context)
        {
            return (await _findEntitiesInListPipeline.Run(new FindEntitiesInListArgument(typeof(SellableItem), listName, 0, take), context).ConfigureAwait(false)).List.Items.OfType<SellableItem>();
        }
    }
}
