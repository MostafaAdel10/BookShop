using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class UpdateOrderStateCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public int OrderStateId { get; set; }
        public UpdateOrderStateCommand(int orderId, int orderStateId)
        {
            OrderId = orderId;
            OrderStateId = orderStateId;
        }
    }
}
