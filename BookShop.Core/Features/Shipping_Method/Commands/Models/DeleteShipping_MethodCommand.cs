using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Shipping_Method.Commands.Models
{
    public class DeleteShipping_MethodCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteShipping_MethodCommand(int id)
        {
            Id = id;
        }
    }
}
