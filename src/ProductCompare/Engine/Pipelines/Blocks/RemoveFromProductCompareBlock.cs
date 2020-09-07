using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Arguments;
using System.Diagnostics.Contracts;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Pipelines.Blocks
{
    public class RemoveFromProductCompareBlock : PipelineBlock<RemoveFromProductCompareArgument, ProductComparison, CommercePipelineExecutionContext>
    {
        private readonly IRemoveListEntitiesPipeline _removeListEntitiesPipeline;

        public RemoveFromProductCompareBlock(IRemoveListEntitiesPipeline removeListEntitiesPipeline)
        {
            _removeListEntitiesPipeline = removeListEntitiesPipeline;
        }

        public override async Task<ProductComparison> Run(RemoveFromProductCompareArgument arg, CommercePipelineExecutionContext context)
        {
            Contract.Requires(arg != null);
            Contract.Requires(context != null);

            Condition.Requires(arg).IsNotNull($"{Name}: The arg can not be null");
            Condition.Requires(arg.SellableItemId).IsNotNull($"{Name}: The product id can not be null");
            Condition.Requires(arg.CompareCollection).IsNotNull($"{Name}: The Compare Collection can not be null");

            var list = arg.CompareCollection.Products.ToList();
            if (list.All(x => x.Id != arg.SellableItemId))
            {
                context.Logger.LogDebug($"{Name}: SellableItem doesn't exist in compare collection, no further action to take");
                return arg.CompareCollection;
            }

            await _removeListEntitiesPipeline.Run(new ListEntitiesArgument(new[]
            {
                arg.SellableItemId
            }, arg.CompareCollection.Name), context.CommerceContext.PipelineContext).ConfigureAwait(false);

            return arg.CompareCollection;
        }
    }
}
