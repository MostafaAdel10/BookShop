using BookShop.Api.Base;
using BookShop.Core.Features.Card_Type.Commands.Models;
using BookShop.Core.Features.Card_Type.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class Cart_TypeController : AppControllerBase
    {
        [HttpGet(Router.Cart_TypeRouting.List)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new GetCart_TypeListQuery());
            return Ok(response);
        }

        [HttpGet(Router.Cart_TypeRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetCart_TypeByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.Cart_TypeRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddCart_TypeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.Cart_TypeRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditCart_TypeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.Cart_TypeRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteCart_TypeCommand(id));
            return NewResult(response);
        }
    }
}
