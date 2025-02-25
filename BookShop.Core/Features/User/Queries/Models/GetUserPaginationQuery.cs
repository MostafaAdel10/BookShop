using BookShop.Core.Features.User.Queries.Response_DTO_;
using BookShop.Core.Wrappers;
using MediatR;

namespace BookShop.Core.Features.User.Queries.Models
{
    public class GetUserPaginationQuery : IRequest<PaginatedResult<GetUserPaginationReponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
