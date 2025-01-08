using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;

namespace BookShop.Core.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : ResponseHandler,
                        IRequestHandler<AddBookCommand, Response<string>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public BookCommandHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            //Mapping between request and book
            var bookMapper = _mapper.Map<Book>(request);
            //Add
            var result = await _bookService.AddAsync(bookMapper);
            //Check condition
            if (result == "ISBN must be unique.") return UnprocessableEntity<string>("ISBN must be unique.");
            if (result == "ISBN must be 13 number.") return UnprocessableEntity<string>("ISBN must be 13 number.");
            //Return response
            if (result == "Success") return Created("Added Successfully.");
            else return BadRequest<string>();
        }
        #endregion

    }
}
