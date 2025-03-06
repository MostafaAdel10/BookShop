using BookShop.Core.Bases;
using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Subject.Queries.Models
{
    public class GetBooksBySubjectIdQuery : IRequest<Response<GetBooksBySubjectIdResponse>>
    {
        public int Id { get; set; }
        public int BookPageNumber { get; set; }
        public int BookPageSize { get; set; }
    }
}
