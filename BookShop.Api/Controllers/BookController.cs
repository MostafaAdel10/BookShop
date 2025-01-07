using BookShop.Core.Features.Books.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("/Book/List")]
        public async Task<IActionResult> GetBooksList()
        {
            var response = await _mediator.Send(new GetBookListQuery());
            return Ok(response);
        }

        [HttpGet("/Book/{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetBookByIdQuery(id));
            return Ok(response);
        }
    }
}
