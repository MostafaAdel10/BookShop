using BookShop.Api.Base;
using BookShop.Core.Features.Subject.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class SubjectController : AppControllerBase
    {
        [HttpGet(Router.SubjectRouting.GetById)]
        public async Task<IActionResult> GetSubjectById([FromQuery] GetSubjectByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
