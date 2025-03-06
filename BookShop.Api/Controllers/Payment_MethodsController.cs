using BookShop.Api.Base;
using BookShop.Core.Features.Payment_Methods.Commands.Models;
using BookShop.Core.Features.Payment_Methods.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class Payment_MethodsController : AppControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.Payment_MethodsRouting.List)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new GetPayment_MethodsListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.Payment_MethodsRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetPayment_MethodsByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.Payment_MethodsRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddPayment_MethodsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.Payment_MethodsRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditPayment_MethodsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.Payment_MethodsRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeletePayment_MethodsCommand(id));
            return NewResult(response);
        }
    }
}
