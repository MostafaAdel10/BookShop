using BookShop.Core.Bases;
using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.SubSubject.Queries.Models
{
    public class GetBooksBySubSubjectIdQuery : IRequest<Response<GetBooksBySubSubjectIdResponse>>
    {
        public int Id { get; set; }
        public int BookPageNumber { get; set; }
        public int BookPageSize { get; set; }
    }
}
