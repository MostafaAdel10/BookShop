using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class AddOrderCommandAPI : IRequest<Response<OrderCommand>>
    {
        public int UserId { get; set; }
        public List<OrderBooks> Books { get; set; } = new List<OrderBooks>();
        public int ShippingMethodId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class OrderBooks
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
