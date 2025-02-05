using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Subject.Commands.Models
{
    public class EditSubjectCommand : IRequest<Response<SubjectCommand>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
    }
}
