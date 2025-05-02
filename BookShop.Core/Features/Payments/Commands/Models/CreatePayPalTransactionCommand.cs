using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Payments.Commands.Models
{
    public class CreatePayPalTransactionCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        /// <summary>
        /// Supported: "EGP", "USD"
        /// </summary>
        public string Currency { get; set; } = "EGP";
        /// <summary>
        /// نوع الدفع: PayPal أو الدفع عند الاستلام
        /// </summary>
    }
}
