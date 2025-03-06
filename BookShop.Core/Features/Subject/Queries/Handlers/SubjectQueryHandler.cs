using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Subject.Queries.Models;
using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookShop.Core.Features.Subject.Queries.Handlers
{
    public class SubjectQueryHandler : ResponseHandler,
        IRequestHandler<GetBooksBySubjectIdQuery, Response<GetBooksBySubjectIdResponse>>,
        IRequestHandler<GetSubjectByIdQuery, Response<GetSubjectByIdResponse>>,
        IRequestHandler<GetSubjectListQuery, Response<List<GetSubjectListResponse>>>

    {
        #region Fields
        private readonly ISubjectService _subjectService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion


        #region Constructors
        public SubjectQueryHandler(ISubjectService subjectService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, IBookService bookService) : base(stringLocalizer)
        {
            _subjectService = subjectService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _bookService = bookService;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<GetBooksBySubjectIdResponse>> Handle(GetBooksBySubjectIdQuery request, CancellationToken cancellationToken)
        {
            // Service GetById Include Books and Subject
            var response = await _subjectService.GetSubjectById(request.Id);

            // Check if the subject does not exist
            if (response == null)
                return NotFound<GetBooksBySubjectIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Mapping response to GetSubjectByIdResponse
            var result = _mapper.Map<GetBooksBySubjectIdResponse>(response);
            var responseList = _mapper.Map<List<GetSubSubjectListResponse>>(response.SubSubjects);

            // Expression for mapping Book to GetBooksListResponse
            Expression<Func<Book, GetBooksListResponse>> bookExpression = e => new GetBooksListResponse(
                e.Id, e.Title, e.Author, e.Price, e.PriceAfterDiscount, e.Publisher,
                e.PublicationDate, e.Unit_Instock, e.Image_url, e.IsActive
            );

            // Retrieve books by subject ID as a queryable
            var bookQueryable = _bookService.GetBookBySubjectIdQueryable(request.Id);

            // Paginate book list
            var paginatedBookList = await bookQueryable.Select(bookExpression)
                .ToPaginatedListAsync(request.BookPageNumber, request.BookPageSize);

            // Assign paginated book list to result
            result.BooksList = paginatedBookList;
            result.SubSubjectsList = responseList;

            // Return successful response with result
            return Success(result);
        }

        public async Task<Response<List<GetSubjectListResponse>>> Handle(GetSubjectListQuery request, CancellationToken cancellationToken)
        {
            var subjectsList = await _subjectService.GetSubjectsListAsync();
            var subjectsListMapper = _mapper.Map<List<GetSubjectListResponse>>(subjectsList);

            var result = Success(subjectsListMapper);
            result.Meta = new { Count = subjectsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSubjectByIdResponse>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await _subjectService.GetByIdAsync(request.Id);

            if (subject == null) return NotFound<GetSubjectByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSubjectByIdResponse>(subject);
            return Success(result);
        }
        #endregion


    }
}
