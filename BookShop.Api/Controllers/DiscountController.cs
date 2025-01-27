using BookShop.Api.Base;
using BookShop.Core.Features.Discount.Commands.Models;
using BookShop.Core.Features.Discount.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class DiscountController : AppControllerBase
    {
        [HttpGet(Router.DiscountRouting.List)]
        public async Task<IActionResult> GetDiscountsList()
        {
            var response = await Mediator.Send(new GetDiscountListQuery());
            return Ok(response);
        }

        [HttpGet(Router.DiscountRouting.GetById)]
        public async Task<IActionResult> GetDiscountById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetDiscountByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.DiscountRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.DiscountRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.DiscountRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteDiscountCommand(id));
            return NewResult(response);
        }
    }
}
