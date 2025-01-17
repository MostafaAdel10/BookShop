using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.Core.Features.Books.Queries.Results;
using BookShop.Core.Resources;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;


namespace BookShop.Core.Features.Books.Queries.Handlers
{
    public class BookQueryHandler : ResponseHandler,
        IRequestHandler<GetBookListQuery, Response<List<GetBookListResponse>>>,
        IRequestHandler<GetBookByIdQuery, Response<GetSingleBookResponse>>,
        IRequestHandler<GetBookPaginatedListQuery, PaginatedResult<GetBookPaginatedListResponse>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion


        #region Constructors
        public BookQueryHandler(IBookService bookService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _bookService = bookService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion


        #region Handel Functions
        public async Task<Response<List<GetBookListResponse>>> Handle(GetBookListQuery request, CancellationToken cancellationToken)
        {
            var booksList = await _bookService.GetBooksListAsync();
            var booksListMapper = _mapper.Map<List<GetBookListResponse>>(booksList);

            var result = Success(booksListMapper);
            result.Meta = new { Count = booksListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleBookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookByIdWithIncludeAsync(request.Id);

            if (book == null) return NotFound<GetSingleBookResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleBookResponse>(book);
            return Success(result);

        }

        public async Task<PaginatedResult<GetBookPaginatedListResponse>> Handle(GetBookPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Book, GetBookPaginatedListResponse>>
                expression = e => new GetBookPaginatedListResponse(e.Id, e.Title, e.Description, e.ISBN13, e.Author, e.Price,
                e.PriceAfterDiscount, e.Publisher, e.PublicationDate, e.Unit_Instock, e.Image_url, e.IsActive,
                e.Subject.Localize(e.Subject.Name_Ar, e.Subject.Name),
                e.SubSubject.Localize(e.SubSubject.Name_Ar, e.SubSubject.Name),
                e.ISBN10);

            var filterQuery = _bookService.FilterBookPaginatedQueryable(request.OrderBy, request.Search);
            var paginatedList = await filterQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
        #endregion


    }
}
