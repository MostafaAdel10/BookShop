using BookShop.Api.Base;
using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.Core.Features.SubSubject.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class SubSubjectController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.SubSubjectRouting.List)]
        public async Task<IActionResult> GetSubSubjectsList()
        {
            var response = await Mediator.Send(new GetSubSubjectListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.SubSubjectRouting.GetBooksBySubSubjectId)]
        public async Task<IActionResult> GetBooksBySubSubjectId([FromQuery] GetBooksBySubSubjectIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.SubSubjectRouting.GetById)]
        public async Task<IActionResult> GetSubSubjectById([FromQuery] GetSubSubjectByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.SubSubjectRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddSubSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.SubSubjectRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditSubSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.SubSubjectRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteSubSubjectCommand(id));
            return NewResult(response);
        }
    }
}
