using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Sitecore.Commerce.Core;
using Plugin.Bootcamp.Exercises.ProductCompare.Commands;

namespace Plugin.Bootcamp.Exercises.ProductCompare.Controllers
{
    [EnableQuery]
    [Route("api/Compare")]
    public class CompareController : CommerceController
    {
        public CompareController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [EnableQuery]
        [HttpGet]
        [Route("(Id={id})")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound();

            var compareComponent = await Command<GetProductCompareCommand>().Process(CurrentContext, id).ConfigureAwait(false);
            return compareComponent != null ? new ObjectResult(compareComponent) : (IActionResult)NotFound();
        }
    }
}
