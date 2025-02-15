using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order_State.Commands.Models
{
    public class AddOrder_StateCommand : IRequest<Response<Order_StateCommand>>
    {
        public string Name { get; set; }
        public string Name_Ar { get; set; }
    }
}
