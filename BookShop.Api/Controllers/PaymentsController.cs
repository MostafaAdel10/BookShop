using BookShop.Api.Base;
using BookShop.Core.Features.Payments.Commands.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class PaymentsController : AppControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpPost(Router.PaymentRouting.PayPalPayment)]
        public async Task<IActionResult> ProcessPayPalPayment([FromBody] CreatePayPalTransactionCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost(Router.PaymentRouting.CashOnDeliveryPayment)]
        public async Task<IActionResult> ProcessCashOnDeliveryPayment([FromBody] CreateCashOnDeliveryTransactionCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.PaymentRouting.UpdateCashOnDeliveryPaymentState)]
        public async Task<IActionResult> UpdateCashOnDeliveryPaymentState([FromBody] UpdateCashOnDeliveryStatusCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
