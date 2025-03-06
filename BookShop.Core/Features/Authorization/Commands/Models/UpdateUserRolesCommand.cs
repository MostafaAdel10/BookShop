using BookShop.Core.Bases;
using BookShop.DataAccess.Requests;
using MediatR;

namespace BookShop.Core.Features.Authorization.Commands.Models
{
    public class UpdateUserRolesCommand : UpdateUserRolesRequest, IRequest<Response<string>>
    {
    }
}
