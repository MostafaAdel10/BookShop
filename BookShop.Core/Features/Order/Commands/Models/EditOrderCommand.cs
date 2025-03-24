using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order.Commands.Models
{
    public class EditOrderCommand : IRequest<Response<int>>
    {
        public int OrderId { get; set; }
        public int ShippingMethodId { get; set; }
        public int PaymentMethodId { get; set; }
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}
