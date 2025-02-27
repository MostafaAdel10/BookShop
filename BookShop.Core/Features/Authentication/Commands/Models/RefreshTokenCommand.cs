using BookShop.Core.Bases;
using BookShop.DataAccess.Results;
using MediatR;

namespace BookShop.Core.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JwtAuthResult>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
