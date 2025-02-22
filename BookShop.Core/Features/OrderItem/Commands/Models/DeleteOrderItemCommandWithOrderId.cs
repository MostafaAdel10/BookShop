using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.OrderItem.Commands.Models
{
    public class DeleteOrderItemCommandWithOrderId : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DeleteOrderItemCommandWithOrderId(int id, int orderId, int userId)
        {
            Id = id;
            OrderId = orderId;
            UserId = userId;
        }
    }
}
