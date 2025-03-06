using BookShop.Api.Base;
using BookShop.Core.Features.Discount.Commands.Models;
using BookShop.Core.Features.Discount.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class DiscountController : AppControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.DiscountRouting.List)]
        public async Task<IActionResult> GetDiscountsList()
        {
            var response = await Mediator.Send(new GetDiscountListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.DiscountRouting.GetById)]
        public async Task<IActionResult> GetDiscountById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetDiscountByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.DiscountRouting.Create)]
        public async Task<IActionResult> Create([FromForm] AddDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.DiscountRouting.Edit)]
        public async Task<IActionResult> Edit([FromForm] EditDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.DiscountRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteDiscountCommand(id));
            return NewResult(response);
        }
    }
}
