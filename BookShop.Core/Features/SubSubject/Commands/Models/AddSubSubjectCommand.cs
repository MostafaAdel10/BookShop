using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.SubSubject.Commands.Models
{
    public class AddSubSubjectCommand : IRequest<Response<SubSubjectCommand>>
    {
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public int SubjectId { get; set; }
    }
}
