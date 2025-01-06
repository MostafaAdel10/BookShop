using BookShop.Core.Features.Books.Queries.Results;
using MediatR;


namespace BookShop.Core.Features.Books.Queries.Models
{
    public class GetBookListQuery : IRequest<List<GetBookListResponse>> 
    {

    }
}
