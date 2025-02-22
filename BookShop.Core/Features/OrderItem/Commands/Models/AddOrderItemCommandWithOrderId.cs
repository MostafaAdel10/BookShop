using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.OrderItem.Commands.Models
{
    public class AddOrderItemCommandWithOrderId : IRequest<Response<OrderItemCommand>>
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int BookId { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
    }
}
