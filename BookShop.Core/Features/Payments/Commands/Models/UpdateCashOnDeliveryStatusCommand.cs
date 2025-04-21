using BookShop.Core.Bases;
using BookShop.DataAccess.Entities;
using MediatR;

namespace BookShop.Core.Features.Payments.Commands.Models
{
    public class UpdateCashOnDeliveryStatusCommand : IRequest<Response<string>>
    {
        /// <summary>
        /// الـ Payment.Id اللي عايزين نحدّثه
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// الحالة الجديدة (مثلاً Completed أو Failed)
        /// </summary>
        public PaymentStatus NewStatus { get; set; }
    }
}
