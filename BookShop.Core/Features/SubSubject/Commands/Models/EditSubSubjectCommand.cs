using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.SubSubject.Commands.Models
{
    public class EditSubSubjectCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public int SubjectId { get; set; }
    }
}
