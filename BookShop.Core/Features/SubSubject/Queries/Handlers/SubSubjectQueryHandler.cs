using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.SubSubject.Queries.Models;
using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookShop.Core.Features.SubSubject.Queries.Handlers
{
    public class SubSubjectQueryHandler : ResponseHandler,
        IRequestHandler<GetSubSubjectByIdQuery, Response<GetSubSubjectByIdResponse>>
    {
        #region Fields
        private readonly ISubSubjectService _subSubjectService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion


        #region Constructors
        public SubSubjectQueryHandler(ISubSubjectService subSubjectService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, IBookService bookService) : base(stringLocalizer)
        {
            _subSubjectService = subSubjectService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _bookService = bookService;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<GetSubSubjectByIdResponse>> Handle(GetSubSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            //Service GetById Include Books and Subject
            var response = await _subSubjectService.GetSubSubjectById(request.Id);
            //Check Is Not Exist
            if (response == null) return NotFound<GetSubSubjectByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            //Mapping
            var result = _mapper.Map<GetSubSubjectByIdResponse>(response);

            Expression<Func<Book, GetBooksListResponses>>
                expression = e => new(e.Id, e.Title, e.Author, e.Price,
                e.PriceAfterDiscount, e.Publisher, e.PublicationDate, e.Unit_Instock, e.Image_url, e.IsActive);

            var bookQuerable = _bookService.GetBookBySubSubjectIdQueryable(request.Id);

            var paginatedList = await bookQuerable.Select(expression).ToPaginatedListAsync(request.BookPageNumber, request.BookPageSize);

            result.BooksList = paginatedList;
            //Return Response
            return Success(result);
        }
        #endregion
    }
}
