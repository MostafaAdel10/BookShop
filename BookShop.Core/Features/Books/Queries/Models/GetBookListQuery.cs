using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Queries.Response_DTO_;
using MediatR;


namespace BookShop.Core.Features.Books.Queries.Models
{
    public class GetBookListQuery : IRequest<Response<List<GetBookListResponse>>>
    {

    }
}
