using BookShop.Api.Base;
using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Features.Order.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class OrderController : AppControllerBase
    {
        [HttpGet(Router.OrderRouting.List)]
        public async Task<IActionResult> GetOrdersList()
        {
            var response = await Mediator.Send(new GetOrderListQuery());
            return Ok(response);
        }

        [HttpGet(Router.OrderRouting.Paginated)]
        public async Task<IActionResult> GetOrdersPaginated([FromQuery] GetOrderPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.OrderRouting.GetById)]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id));
            return NewResult(response);
        }

        [HttpGet(Router.OrderRouting.GetOrdersByUserId)]
        public async Task<IActionResult> GetOrdersByUserId([FromRoute] int userId)
        {
            var response = await Mediator.Send(new GetOrdersByUserIdQuery(userId));
            return NewResult(response);
        }

        [HttpPost(Router.OrderRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddOrderCommandAPI command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.OrderRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditOrderCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.OrderRouting.EditOrderState)]
        public async Task<IActionResult> EditOrderState([FromBody] UpdateOrderStateCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.OrderRouting.CancelOrder)]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            var response = await Mediator.Send(new CancelOrderCommand(id));
            return NewResult(response);
        }
    }
}
