using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.DataAccess.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    //[Route("api/[controller]")] //I Made Custom Routing
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet(Router.BookRouting.List)]
        public async Task<IActionResult> GetBooksList()
        {
            var response = await _mediator.Send(new GetBookListQuery());
            return Ok(response);
        }

        [HttpGet(Router.BookRouting.GetById)]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetBookByIdQuery(id));
            return Ok(response);
        }

        [HttpPost(Router.BookRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddBookCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
