using BookShop.Core.Bases;
using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.SubSubject.Queries.Models
{
    public class GetSubSubjectListQuery : IRequest<Response<List<GetSubSubjectListResponses>>>
    {
    }
}
