using BookShop.Api.Base;
using BookShop.Core.Features.ShippingAddress.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    public class ShippingAddressController : AppControllerBase
    {
        [HttpGet(Router.ShippingAddressRouting.GetCurrentUser_sShippingAddresses)]
        public async Task<IActionResult> GetCurrentUser_sShippingAddresses()
        {
            var response = await Mediator.Send(new GetShippingAddressesByCurrentUserIdQuery());
            return Ok(response);
        }
    }
}
