using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.OData;
using Plugin.Bootcamp.Exercises.ProductCompare.Commands;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Controllers
{
    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("AddProductToComparison()")]
        public async Task<IActionResult> AddProductToComparison([FromBody] ODataActionParameters value)
        {
            if (!ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (!value.ContainsKey("cartId") || !value.ContainsKey("productId") ||
                !value.ContainsKey("catalogName") || !value.ContainsKey("variantId"))
            {
                return new BadRequestObjectResult(value);
            }

            var cartId = value["cartId"].ToString();
            var catalogName = value["catalogName"].ToString();
            var productId = value["productId"].ToString();
            var variantId = value["variantId"].ToString();

            if (string.IsNullOrWhiteSpace(cartId) || string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(catalogName))
            {
                return new BadRequestObjectResult(value);
            }

            var command = Command<AddToProductCompareCommand>();
            await command.Process(CurrentContext, cartId, catalogName, productId, variantId).ConfigureAwait(false);
            return new ObjectResult(command);
        }


        [HttpDelete]
        [Route("RemoveProductFromComparison()")]
        public async Task<IActionResult> RemoveProductFromComparison([FromBody] ODataActionParameters value)
        {
            if (!ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (!value.ContainsKey("cartId") || !value.ContainsKey("sellableItemId"))
            {
                return new BadRequestObjectResult(value);
            }

            var cartId = value["cartId"].ToString();
            var sellableItemId = value["sellableItemId"].ToString();
            if (string.IsNullOrWhiteSpace(cartId) || string.IsNullOrWhiteSpace(sellableItemId))
            {
                return new BadRequestObjectResult(value);
            }

            var command = Command<RemoveFromProductCompareCommand>();
            await command.Process(CurrentContext, cartId, sellableItemId).ConfigureAwait(false);
            return new ObjectResult(command);
        }
    }
}
