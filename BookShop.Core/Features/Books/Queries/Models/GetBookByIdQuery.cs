using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Queries.Results;
using MediatR;


namespace BookShop.Core.Features.Books.Queries.Models
{
    public class GetBookByIdQuery : IRequest<Response<GetSingleBookResponse>>
    {
        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
