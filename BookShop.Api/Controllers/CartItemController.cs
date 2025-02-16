using BookShop.Api.Base;
using BookShop.Core.Features.CartItem.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class CartItemController : AppControllerBase
    {
        [HttpGet(Router.CartItemRouting.List)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new GetCartItemListQuery());
            return Ok(response);
        }

        [HttpGet(Router.CartItemRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetCartItemByIdQuery(id));
            return NewResult(response);
        }
    }
}
