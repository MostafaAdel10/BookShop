using BookShop.Core.Features.Order.Queries.Models;

namespace BookShop.Core.Features.Order.Queries.Response_DTO_
{
    public class GetOrderPaginatedListResponse
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string TrackingNumber { get; set; }
        public ShippingAddressQuery ShippingAddress { get; set; }
        public ICollection<OrderItemQuery> OrderItems { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OrderState { get; set; }
        public string? OrderStateArabic { get; set; }
        public decimal? ShippingCost { get; set; }
    }
}
