using BookShop.Core.Bases;
using BookShop.DataAccess.Results;
using MediatR;

namespace BookShop.Core.Features.Authorization.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManageUserClaimsResult>>
    {
        public int UserId { get; set; }
    }
}
