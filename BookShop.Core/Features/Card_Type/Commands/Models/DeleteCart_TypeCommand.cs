using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Card_Type.Commands.Models
{
    public class DeleteCart_TypeCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCart_TypeCommand(int id)
        {
            Id = id;
        }
    }
}
