using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Plugin.Bootcamp.Exercises.ProductCompare.Pipelines;
using System.Diagnostics.Contracts;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Commands
{
    public class GetProductCompareCommand : CommerceCommand
    {
        private readonly IGetProductComparePipeline _getProductComparePipeline;

        public GetProductCompareCommand(IGetProductComparePipeline getProductComparePipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _getProductComparePipeline = getProductComparePipeline;
        }

        public virtual async Task<ProductComparison> Process(CommerceContext context, string cartId)
        {
            return await GetProductCompare(context, cartId).ConfigureAwait(false);
        }

        protected virtual async Task<ProductComparison> GetProductCompare(CommerceContext context, string cartId)
        {
            Contract.Requires(context != null);

            if (string.IsNullOrEmpty(cartId))
            {
                return null;
            }

            var entityPrefix = CommerceEntity.IdPrefix<ProductComparison>();
            var entityId = cartId.StartsWith(entityPrefix, StringComparison.OrdinalIgnoreCase) ? cartId : $"{entityPrefix}{cartId}";
            System.Diagnostics.Debug.WriteLine($"Entity Id: {entityId}", "Instrumentation");
            var options = new CommercePipelineExecutionContextOptions(context);

            var productComparison = await _getProductComparePipeline.Run(entityId, options).ConfigureAwait(false);
            if (productComparison == null)
            {
                context.Logger.LogDebug($"Entity {entityId} was not found.");
            }

            return productComparison;
        }
    }
}
