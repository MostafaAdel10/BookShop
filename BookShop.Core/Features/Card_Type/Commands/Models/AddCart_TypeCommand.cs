using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Card_Type.Commands.Models
{
    public class AddCart_TypeCommand : IRequest<Response<Cart_TypeCommand>>
    {
        public string Name { get; set; }
    }
}
