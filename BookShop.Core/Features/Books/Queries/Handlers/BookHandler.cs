using AutoMapper;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.Core.Features.Books.Queries.Results;
using BookShop.Service.Abstract;
using MediatR;


namespace BookShop.Core.Features.Books.Queries.Handlers
{
    public class BookHandler : IRequestHandler<GetBookListQuery, List<GetBookListResponse>>
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
        public async Task<List<GetBookListResponse>> Handle(GetBookListQuery request, CancellationToken cancellationToken)
        {
            var booksList = await _bookService.GetBooksListAsync();
            var booksListMapper = _mapper.Map<List<GetBookListResponse>>(booksList);

            return booksListMapper;
        }
        #endregion


    }
}
