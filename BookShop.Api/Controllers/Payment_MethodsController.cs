using BookShop.Api.Base;
using BookShop.Core.Features.Payment_Methods.Commands.Models;
using BookShop.Core.Features.Payment_Methods.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class Payment_MethodsController : AppControllerBase
    {
        [HttpGet(Router.Payment_MethodsRouting.List)]
        public async Task<IActionResult> GetList()
        {
            var response = await Mediator.Send(new GetPayment_MethodsListQuery());
            return Ok(response);
        }

        [HttpGet(Router.Payment_MethodsRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetPayment_MethodsByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.Payment_MethodsRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddPayment_MethodsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.Payment_MethodsRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditPayment_MethodsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.Payment_MethodsRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeletePayment_MethodsCommand(id));
            return NewResult(response);
        }
    }
}
