using BookShop.Api.Base;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    //[Route("api/[controller]")] //I Made Custom Routing
    [ApiController]
    public class BookController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.BookRouting.List)]
        public async Task<IActionResult> GetBooksList()
        {
            var response = await Mediator.Send(new GetBookListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.BookRouting.Paginated)]
        public async Task<IActionResult> GetBooksPaginated([FromQuery] GetBookPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.BookRouting.GetById)]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetBookByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.BookRouting.Create)]
        public async Task<IActionResult> Create([FromForm] AddBookCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.BookRouting.CreateImages)]
        public async Task<IActionResult> CreateImages([FromForm] AddImagesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.BookRouting.Edit)]
        public async Task<IActionResult> Edit([FromForm] EditBookCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.BookRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteBookCommand(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.BookRouting.DeleteImageFromBook)]
        public async Task<IActionResult> DeleteImageFromBook([FromForm] DeleteImageFromBookCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.BookRouting.DeleteDiscountFromBooks)]
        public async Task<IActionResult> DeleteDiscountFromBooks([FromRoute] int discountId)
        {
            var response = await Mediator.Send(new DeleteDiscountFromBooksCommand(discountId));
            return NewResult(response);
        }
    }
}
