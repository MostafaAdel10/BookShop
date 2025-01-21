using BookShop.Api.Base;
using BookShop.Core.Features.SubSubject.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class SubSubjectController : AppControllerBase
    {
        [HttpGet(Router.SubSubjectRouting.GetById)]
        public async Task<IActionResult> GetSubSubjectById([FromQuery] GetSubSubjectByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
