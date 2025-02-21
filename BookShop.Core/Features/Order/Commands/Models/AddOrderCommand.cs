using BookShop.Core.Features.OrderItem.Commands.Models;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class AddOrderCommand
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public ICollection<AddOrderItemCommand> OrderItems { get; set; } = new List<AddOrderItemCommand>();
        public int ShippingMethodId { get; set; }
        public int PaymentMethodId { get; set; }
        public int UserId { get; set; }
        public int OrderStateId { get; set; }
    }
}
