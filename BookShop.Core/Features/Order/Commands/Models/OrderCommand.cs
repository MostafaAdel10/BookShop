using BookShop.Core.Features.OrderItem.Commands.Models;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class OrderCommand
    {
        public OrderCommand()
        {


        }
        public OrderCommand(DataAccess.Entities.Order order)
        {
            Id = order.Id;
            OrderDate = order.OrderDate;
            TotalAmount = order.Total_amout;
            TrackingNumber = order.tracking_number;
            OrderItems = order.OrderItems!.Select(p => new OrderItemCommand
            {
                Id = p.Id,
                Quantity = p.Quantity,
                OrderId = p.OrderId,
                Price = p.Price,
                Tax = p.Tax,
                BookId = p.BookId,
                BookName = p.book != null ? p.book.Title : string.Empty
            }).ToList();
            ShippingAddress = order.shipping_address;
            UserId = order.ApplicationUserId;

            OrderState = order.order_State != null ? order.order_State.Name! : "Pending";
            OrderStateArabic = order.order_State!.Name_Ar ?? "قيد الانتظار";
            ShippingMethod = order.shipping_Methods!.Method_Name;
            PaymentMethod = order.payment_Methods!.Name ?? "Paypal";
            Code = order.Code;
            UserName = order.ApplicationUser != null ? order.ApplicationUser.UserName ?? string.Empty : string.Empty;
            ShippingCost = order.shipping_Methods.Cost;

        }
        public int Id { get; set; }
        public int? Code { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingAddress { get; set; }
        public ICollection<OrderItemCommand> OrderItems { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OrderState { get; set; }
        public string? OrderStateArabic { get; set; }
        public decimal? ShippingCost { get; set; }
    }
    public record OrderStatusDTO
    {
        public int orderId { get; set; }
        public string? OrderStatus { get; set; }
        public Dictionary<string, int>? orderStat { get; set; }
    }
}

