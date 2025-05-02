using BookShop.Api.Base;
using BookShop.Core.Features.Payments.Commands.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class MobileCashPaymentsController : AppControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost(Router.PaymentRouting.VodafoneCashTransaction)]
        public async Task<IActionResult> VodafoneCashTransaction([FromBody] CreateVodafoneCashTransactionCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost(Router.PaymentRouting.EtisalatCashTransaction)]
        public async Task<IActionResult> EtisalatCashTransaction([FromBody] CreateEtisalatCashTransactionCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
