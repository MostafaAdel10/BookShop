using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class CancelOrderCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public CancelOrderCommand(int orderId)
        {
            OrderId = orderId;
        }
    }
}
