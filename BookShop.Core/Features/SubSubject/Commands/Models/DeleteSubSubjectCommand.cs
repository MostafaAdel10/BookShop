using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.SubSubject.Commands.Models
{
    public class DeleteSubSubjectCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteSubSubjectCommand(int id)
        {
            Id = id;
        }
    }
}
