using BookShop.Api.Base;
using BookShop.Core.Features.Review.Commands.Models;
using BookShop.Core.Features.Review.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class ReviewController : AppControllerBase
    {
        [HttpGet(Router.ReviewRouting.List)]
        public async Task<IActionResult> GetReviewsList()
        {
            var response = await Mediator.Send(new GetReviewListQuery());
            return Ok(response);
        }

        [HttpGet(Router.ReviewRouting.GetById)]
        public async Task<IActionResult> GetReviewById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetReviewByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.ReviewRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.ReviewRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ReviewRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteReviewCommand(id));
            return NewResult(response);
        }
    }
}
