using BookShop.Api.Base;
using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.Core.Features.Subject.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class SubjectController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.SubjectRouting.List)]
        public async Task<IActionResult> GetSubjectsList()
        {
            var response = await Mediator.Send(new GetSubjectListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.SubjectRouting.GetBooksBySubjectId)]
        public async Task<IActionResult> GetBooksBySubjectId([FromQuery] GetBooksBySubjectIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.SubjectRouting.GetById)]
        public async Task<IActionResult> GetSubjectById([FromQuery] GetSubjectByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.SubjectRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.SubjectRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.SubjectRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteSubjectCommand(id));
            return NewResult(response);
        }
    }
}
