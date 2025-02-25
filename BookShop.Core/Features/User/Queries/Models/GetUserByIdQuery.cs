using BookShop.Core.Bases;
using BookShop.Core.Features.User.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.User.Queries.Models
{
    public class GetUserByIdQuery : IRequest<Response<GetUserByIdResponse>>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
