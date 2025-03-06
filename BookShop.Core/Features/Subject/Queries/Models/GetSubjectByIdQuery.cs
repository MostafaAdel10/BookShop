using BookShop.Core.Bases;
using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Subject.Queries.Models
{
    public class GetSubjectByIdQuery : IRequest<Response<GetSubjectByIdResponse>>
    {
        public GetSubjectByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
