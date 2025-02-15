using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Order_State.Commands.Models
{
    public class DeleteOrder_StateCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteOrder_StateCommand(int id)
        {
            Id = id;
        }
    }
}
