using BookShop.Core.Bases;
using BookShop.Core.Features.Authorization.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Authorization.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<GetRoleByIdResponse>>
    {
        public int Id { get; set; }
    }
}
