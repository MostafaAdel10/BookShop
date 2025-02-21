using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class EditOrderCommand : IRequest<Response<OrderCommand>>
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        //public DateTime OrderDate { get; set; } = DateTime.Now;
        //public decimal TotalAmount { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingAddress { get; set; }
        //public ICollection<EditOrderItemCommand> OrderItems { get; set; } = new List<EditOrderItemCommand>();
        public int ShippingMethodId { get; set; }
        //public int PaymentMethodId { get; set; }
        public int OrderStateId { get; set; }
    }
}
