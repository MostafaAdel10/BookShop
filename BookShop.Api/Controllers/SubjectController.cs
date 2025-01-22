using BookShop.Api.Base;
using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.Core.Features.Subject.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class SubjectController : AppControllerBase
    {
        [HttpGet(Router.SubjectRouting.List)]
        public async Task<IActionResult> GetSubjectsList()
        {
            var response = await Mediator.Send(new GetSubjectListQuery());
            return Ok(response);
        }

        [HttpGet(Router.SubjectRouting.GetById)]
        public async Task<IActionResult> GetSubjectById([FromQuery] GetSubjectByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.SubjectRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
