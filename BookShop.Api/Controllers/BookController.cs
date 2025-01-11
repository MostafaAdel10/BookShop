using BookShop.Api.Base;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using FluentValidation;
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

        [HttpGet(Router.BookRouting.GetById)]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetBookByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.BookRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddBookCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
                return NewResult(response);
            }
            catch (ValidationException ex)
            {
                // Handle ValidationException specifically
                return UnprocessableEntity(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

    }
}
