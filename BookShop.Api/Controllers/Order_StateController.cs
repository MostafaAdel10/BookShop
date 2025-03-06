using BookShop.Api.Base;
using BookShop.Core.Features.Order_State.Commands.Models;
using BookShop.Core.Features.Order_State.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class Order_StateController : AppControllerBase
    {
        [HttpGet(Router.Order_StateRouting.List)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new GetOrder_StateListQuery());
            return Ok(response);
        }

        [HttpGet(Router.Order_StateRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetOrder_StateByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.Order_StateRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddOrder_StateCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.Order_StateRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditOrder_StateCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.Order_StateRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteOrder_StateCommand(id));
            return NewResult(response);
        }
    }
}
