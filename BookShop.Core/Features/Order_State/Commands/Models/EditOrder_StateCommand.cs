using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order_State.Commands.Models
{
    public class EditOrder_StateCommand : IRequest<Response<Order_StateCommand>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
    }
}
