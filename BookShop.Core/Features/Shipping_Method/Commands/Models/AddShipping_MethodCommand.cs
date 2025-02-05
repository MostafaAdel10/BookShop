using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Shipping_Method.Commands.Models
{
    public class AddShipping_MethodCommand : IRequest<Response<Shipping_MethodCommand>>
    {
        public string Method_Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime Estimated_Delivery_Time { get; set; }
    }
}
