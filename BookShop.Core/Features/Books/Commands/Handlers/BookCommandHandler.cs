using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : ResponseHandler,
                        IRequestHandler<AddBookCommand, Response<string>>,
                        IRequestHandler<EditBookCommand, Response<string>>,
                        IRequestHandler<DeleteBookCommand, Response<string>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public BookCommandHandler(IBookService bookService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _bookService = bookService;
            _mapper = mapper;
            _localizer = stringLocalizer;
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
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<string>();
            //Mapping between request and book
            var bookMapper = _mapper.Map<Book>(request);
            //Call service that make edit
            var result = await _bookService.EditAsync(bookMapper);
            //Return response
            if (result == "Success")
                return Success((string)_localizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<string>();
            //Call service that make delete
            var result = await _bookService.DeleteAsync(book);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();

        }
        #endregion

    }
}
