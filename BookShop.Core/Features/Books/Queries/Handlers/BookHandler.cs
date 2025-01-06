using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.Core.Features.Books.Queries.Results;
using BookShop.Service.Abstract;
using MediatR;


namespace BookShop.Core.Features.Books.Queries.Handlers
{
    public class BookHandler : ResponseHandler, IRequestHandler<GetBookListQuery, Response<List<GetBookListResponse>>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        #endregion
        

        #region Constructors
        public BookHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        #endregion


        #region Handel Functions
        public async Task<Response<List<GetBookListResponse>>> Handle(GetBookListQuery request, CancellationToken cancellationToken)
        {
            var booksList = await _bookService.GetBooksListAsync();
            var booksListMapper = _mapper.Map<List<GetBookListResponse>>(booksList);

            return Success(booksListMapper);
        }
        #endregion


    }
}
