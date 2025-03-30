using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Queries.Models;
using BookShop.Core.Features.Review.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Core.Wrappers;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookShop.Core.Features.Review.Queries.Handlers
{
    public class ReviewQueryHandler : ResponseHandler,
            IRequestHandler<GetReviewListQuery, Response<List<GetReviewListResponse>>>,
            IRequestHandler<GetReviewByIdQuery, Response<GetSingleReviewResponse>>,
            IRequestHandler<GetReviewPaginatedListQuery, PaginatedResult<GetReviewPaginatedListResponse>>,
            IRequestHandler<GetReviewByBookIdQuery, Response<List<GetReviewsListByBookIdResponse>>>,
            IRequestHandler<GetReviewsByCurrentUserQuery, Response<List<GetReviewListResponse>>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ReviewQueryHandler(IReviewService reviewService, IMapper mapper, ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetReviewListResponse>>> Handle(GetReviewListQuery request, CancellationToken cancellationToken)
        {
            var reviewsList = await _reviewService.GetReviewsListAsync();
            var reviewsListMapper = _mapper.Map<List<GetReviewListResponse>>(reviewsList);

            var result = Success(reviewsListMapper);
            result.Meta = new { Count = reviewsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleReviewResponse>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _reviewService.GetReviewByIdAsync(request.Id);

            if (review == null) return NotFound<GetSingleReviewResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleReviewResponse>(review);
            return Success(result);
        }

        public async Task<PaginatedResult<GetReviewPaginatedListResponse>> Handle(GetReviewPaginatedListQuery request, CancellationToken cancellationToken)
        {
            //pagination
            Expression<Func<DataAccess.Entities.Review, GetReviewPaginatedListResponse>>
                expression = e => new GetReviewPaginatedListResponse(e.Id, e.Rating, e.Content, e.BookId);

            var filterQuery = _reviewService.FilterReviewPaginatedQueryable(request.OrderBy, request.Search);
            var paginatedList = await filterQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }

        public async Task<Response<List<GetReviewsListByBookIdResponse>>> Handle(GetReviewByBookIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetReviewsListAsyncQueryble();
            var bookReviews = reviews.Where(r => r.BookId == request.BookId).ToList();
            if (bookReviews == null || !bookReviews.Any())
                return NotFound<List<GetReviewsListByBookIdResponse>>(_stringLocalizer[SharedResourcesKeys.NoReviews]);
            var bookReviewsListMapper = bookReviews.Select(review => new GetReviewsListByBookIdResponse
            {
                Id = review.Id,
                BookId = review.BookId,
                Rating = review.Rating,
                Content = review.Content,
                UserName = review.UserReviews.FirstOrDefault()?.applicationUser.UserName
            }).ToList();
            var result = Success(bookReviewsListMapper);
            result.Meta = new { Count = bookReviewsListMapper.Count() };
            return result;
        }

        public async Task<Response<List<GetReviewListResponse>>> Handle(GetReviewsByCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var reviews = await _reviewService.GetReviewsListAsyncQueryble();
            var userReviews = reviews.Where(r => r.UserReviews != null && r.UserReviews.Any(ur => ur.ApplicationUserId == currentUserId)).ToList();
            var usrtReviewsListMapper = _mapper.Map<List<GetReviewListResponse>>(userReviews);
            var result = Success(usrtReviewsListMapper);
            result.Meta = new { Count = usrtReviewsListMapper.Count() };
            return result;
        }
        #endregion
    }
}
