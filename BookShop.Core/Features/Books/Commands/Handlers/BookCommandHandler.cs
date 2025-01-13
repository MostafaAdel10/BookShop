using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;

namespace BookShop.Core.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : ResponseHandler,
                        IRequestHandler<AddBookCommand, Response<string>>,
                        IRequestHandler<EditBookCommand, Response<string>>
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

            if (result == "Success")
                return Created("Added Successfully.");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetBookByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<string>("Book Is Not Found.");
            //Mapping between request and book
            var bookMapper = _mapper.Map<Book>(request);
            //Call service that make edit
            var result = await _bookService.EditAsync(bookMapper);
            //Return response
            if (result == "Success")
                return Success($"Edit Successfully.{bookMapper.Id}");
            else
                return BadRequest<string>();
        }
        #endregion

    }
}
