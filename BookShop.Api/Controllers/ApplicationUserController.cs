using BookShop.Api.Base;
using BookShop.Core.Features.User.Commands.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin,User")]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPost(Router.ApplicationUserRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
