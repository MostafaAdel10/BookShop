using BookShop.Core.Bases;
using BookShop.Core.Features.CartItem.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.CartItem.Queries.Models
{
    public class GetCartItemByIdQuery : IRequest<Response<GetSingleCartItemResponse>>
    {
        public GetCartItemByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
