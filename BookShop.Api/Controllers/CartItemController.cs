using BookShop.Api.Base;
using BookShop.Core.Features.CartItem.Commands.Models;
using BookShop.Core.Features.CartItem.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    public class CartItemController : AppControllerBase
    {
        [HttpGet(Router.CartItemRouting.GetCurrentUser_sCartItems)]
        public async Task<IActionResult> GetCurrentUser_sCartItems()
        {
            var response = await Mediator.Send(new GetCartItemsByCurrentUserIdQuery());
            return Ok(response);
        }

        [HttpPost(Router.CartItemRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.CartItemRouting.EditTheCartItemQuantityAndCheckIfItIsInStockCommand)]
        public async Task<IActionResult> EditTheCartItemQuantityAndCheckIfItIsInStockCommand([FromBody] EditTheCartItemQuantityAndCheckIfItIsInStockCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.CartItemRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteCartItemCommand(id));
            return NewResult(response);
        }
    }
}
