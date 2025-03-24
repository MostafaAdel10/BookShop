using BookShop.Api.Base;
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
    }
}
