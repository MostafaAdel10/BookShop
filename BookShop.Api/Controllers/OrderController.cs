using BookShop.Api.Base;
using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Features.Order.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class OrderController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.OrderRouting.List)]
        public async Task<IActionResult> GetOrdersList()
        {
            var response = await Mediator.Send(new GetOrderListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.OrderRouting.Paginated)]
        public async Task<IActionResult> GetOrdersPaginated([FromQuery] GetOrderPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.OrderRouting.GetById)]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "User")]
        [HttpGet(Router.OrderRouting.GetOrdersByCurrentUser)]
        public async Task<IActionResult> GetOrdersByCurrentUser()
        {
            var response = await Mediator.Send(new GetOrdersByCurrentUserIdQuery());
            return NewResult(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost(Router.OrderRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddOrderCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut(Router.OrderRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditOrderCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.OrderRouting.EditOrderState)]
        public async Task<IActionResult> EditOrderState([FromBody] UpdateOrderStateCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete(Router.OrderRouting.CancelOrder)]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            var response = await Mediator.Send(new CancelOrderCommand(id));
            return NewResult(response);
        }
    }
}
