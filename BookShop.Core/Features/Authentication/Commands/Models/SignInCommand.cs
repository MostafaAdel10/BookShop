using BookShop.Core.Bases;
using BookShop.DataAccess.Results;
using MediatR;

namespace BookShop.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<Response<JwtAuthResult>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
