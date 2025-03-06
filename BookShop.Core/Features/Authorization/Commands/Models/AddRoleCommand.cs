using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Authorization.Commands.Models
{
    public class AddRoleCommand : IRequest<Response<string>>
    {
        public string RoleName { get; set; }
    }
}
