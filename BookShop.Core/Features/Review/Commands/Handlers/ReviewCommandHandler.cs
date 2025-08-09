using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Review.Commands.Handlers
{
    public class ReviewCommandHandler : ResponseHandler,
            IRequestHandler<AddReviewCommand, Response<ReviewCommand>>,
            IRequestHandler<EditReviewCommand, Response<ReviewCommand>>,
            IRequestHandler<DeleteReviewCommand, Response<string>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IBookService _bookService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUser_ReviewsService _user_ReviewsService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ReviewCommandHandler(IReviewService reviewService, IMapper mapper, ICurrentUserService currentUserService,
            IBookService bookService, IUser_ReviewsService user_ReviewsService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _bookService = bookService;
            _currentUserService = currentUserService;
            _user_ReviewsService = user_ReviewsService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<ReviewCommand>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            // Validate Product and User existence
            var currentUserId = _currentUserService.GetUserId();

            var book = await _bookService.GetByIdAsync(request.BookId);
            if (book is null)
                return NotFound<ReviewCommand>(_localizer[SharedResourcesKeys.NotFound]);

            //Mapping between request and Review
            var review = _mapper.Map<DataAccess.Entities.Review>(request);

            //Add
            var result = await _reviewService.AddAsyncWithReturnId(review);

            if (result is not null)
            {
                //  ربط المراجعة بالمستخدم
                var userReview = new User_Reviews
                {
                    ApplicationUserId = currentUserId,
                    ReviewID = result.Id
                };

                await _user_ReviewsService.AddAsync(userReview);

                //  تحويل الكائن إلى DTO والاستجابة
                var returnReview = _mapper.Map<ReviewCommand>(review);
                return Created(returnReview);
            }

            //  في حال الفشل
            return BadRequest<ReviewCommand>(_localizer[SharedResourcesKeys.FailedToAdd]);
        }

        public async Task<Response<ReviewCommand>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            //Check if the id is exist or not
            var review = await _reviewService.GetReviewByIdAsyncWithInclude(request.Id);
            if (review is null)
                return NotFound<ReviewCommand>();

            // التحقق من ملكية المراجعة
            var isOwnedByCurrentUser = review.UserReviews.Any(ur => ur.ApplicationUserId == currentUserId);
            if (!isOwnedByCurrentUser)
                return Unauthorized<ReviewCommand>(_localizer[SharedResourcesKeys.UnAuthorized]);

            //Mapping between request and Review
            _mapper.Map(request, review);

            //Call service that make edit
            var result = await _reviewService.EditAsync(review);

            //Return response
            return result == "Success"
            ? Success(_mapper.Map<ReviewCommand>(review), _localizer[SharedResourcesKeys.Updated])
            : BadRequest<ReviewCommand>(_localizer[SharedResourcesKeys.FailedToUpdate]);
        }

        public async Task<Response<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var review = await _reviewService.GetReviewByIdAsync(request.Id);
            if (review is null)
                return NotFound<string>();

            //Call service that make delete
            var result = await _reviewService.DeleteAsync(review);

            //Return response
            return result == "Success"
            ? Deleted<string>()
            : BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDelete]);
        }
        #endregion
    }
}
