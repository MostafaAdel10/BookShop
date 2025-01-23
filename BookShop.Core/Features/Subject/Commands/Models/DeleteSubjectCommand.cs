using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Subject.Commands.Models
{
    public class DeleteSubjectCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteSubjectCommand(int id)
        {
            Id = id;
        }
    }
}
