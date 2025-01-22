using BookShop.Core.Bases;
using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Subject.Queries.Models
{
    public class GetSubjectListQuery : IRequest<Response<List<GetSubjectListResponse>>>
    {
    }
}
