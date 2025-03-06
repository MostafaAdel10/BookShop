using BookShop.Api.Base;
using BookShop.Core.Features.Review.Commands.Models;
using BookShop.Core.Features.Review.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [ApiController]
    public class ReviewController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ReviewRouting.List)]
        public async Task<IActionResult> GetReviewsList()
        {
            var response = await Mediator.Send(new GetReviewListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ReviewRouting.Paginated)]
        public async Task<IActionResult> GetReviewsPaginated()
        {
            var response = await Mediator.Send(new GetReviewPaginatedListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ReviewRouting.GetById)]
        public async Task<IActionResult> GetReviewById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetReviewByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.ReviewRouting.GetByBookId)]
        public async Task<IActionResult> GetReviewByBookId([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetReviewByBookIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.ReviewRouting.GetByUserId)]
        public async Task<IActionResult> GetReviewByUserId([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetReviewByUserIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost(Router.ReviewRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut(Router.ReviewRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete(Router.ReviewRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteReviewCommand(id));
            return NewResult(response);
        }
    }
}
