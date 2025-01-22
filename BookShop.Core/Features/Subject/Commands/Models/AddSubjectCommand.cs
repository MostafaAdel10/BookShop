using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Subject.Commands.Models
{
    public class AddSubjectCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string Name_Ar { get; set; }
    }
}
