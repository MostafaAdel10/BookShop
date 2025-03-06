using BookShop.Api.Base;
using BookShop.Core.Features.OrderItem.Commands.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class OrderItemController : AppControllerBase
    {
        [HttpPost(Router.OrderItemRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddOrderItemCommandWithOrderId command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.OrderItemRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditOrderItemCommandWithOrderId command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.OrderItemRouting.Delete)]
        public async Task<IActionResult> Delete([FromBody] DeleteOrderItemCommandWithOrderId command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}
