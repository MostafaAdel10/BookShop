using BookShop.Api.Base;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    //[Route("api/[controller]")] //I Made Custom Routing
    [ApiController]
    public class BookController : AppControllerBase
    {
        [HttpGet(Router.BookRouting.List)]
        public async Task<IActionResult> GetBooksList()
        {
            var response = await Mediator.Send(new GetBookListQuery());
            return Ok(response);
        }

        [HttpGet(Router.BookRouting.Paginated)]
        public async Task<IActionResult> GetBooksPaginated([FromQuery] GetBookPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.BookRouting.GetById)]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetBookByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.BookRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddBookCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.BookRouting.CreateImages)]
        public async Task<IActionResult> CreateImages([FromBody] AddImagesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.BookRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditBookCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.BookRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteBookCommand(id));
            return NewResult(response);
        }

        [HttpDelete(Router.BookRouting.DeleteImageFromBook)]
        public async Task<IActionResult> DeleteImageFromBook([FromRoute] int bookId, [FromRoute] string imageUrl)
        {
            var response = await Mediator.Send(new DeleteImageFromBookCommand(bookId, imageUrl));
            return NewResult(response);
        }
    }
}
