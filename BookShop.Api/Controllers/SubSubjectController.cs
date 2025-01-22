using BookShop.Api.Base;
using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.Core.Features.SubSubject.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class SubSubjectController : AppControllerBase
    {
        [HttpGet(Router.SubSubjectRouting.List)]
        public async Task<IActionResult> GetSubSubjectsList()
        {
            var response = await Mediator.Send(new GetSubSubjectListQuery());
            return Ok(response);
        }

        [HttpGet(Router.SubSubjectRouting.GetById)]
        public async Task<IActionResult> GetSubSubjectById([FromQuery] GetSubSubjectByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.SubSubjectRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddSubSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
