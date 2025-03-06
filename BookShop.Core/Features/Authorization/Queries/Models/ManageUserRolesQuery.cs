using BookShop.Core.Bases;
using BookShop.DataAccess.Results;
using MediatR;

namespace BookShop.Core.Features.Authorization.Queries.Models
{
    public class ManageUserRolesQuery : IRequest<Response<ManageUserRolesResult>>
    {
        public int UserId { get; set; }
    }
}
