using BookShop.Api.Base;
using BookShop.Core.Features.Shipping_Method.Commands.Models;
using BookShop.Core.Features.Shipping_Method.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class Shipping_MethodController : AppControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.Shipping_MethodRouting.List)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new GetShipping_MethodListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.Shipping_MethodRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetShipping_MethodByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.Shipping_MethodRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddShipping_MethodCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.Shipping_MethodRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditShipping_MethodCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.Shipping_MethodRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteShipping_MethodCommand(id));
            return NewResult(response);
        }
    }
}
