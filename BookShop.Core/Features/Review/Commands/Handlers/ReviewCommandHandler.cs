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
            var book = await _bookService.GetByIdAsync(request.BookId);
            var currentUserId = _currentUserService.GetUserId();

            if (book == null) return NotFound<ReviewCommand>(_localizer[SharedResourcesKeys.InvalidFileType]);

            //Mapping between request and Review
            var reviewMapper = _mapper.Map<DataAccess.Entities.Review>(request);
            //Add
            var result = await _reviewService.AddAsyncWithReturnId(reviewMapper);

            if (result != null)
            {
                var user_Reviews = new User_Reviews { ApplicationUserId = currentUserId, ReviewID = reviewMapper.Id };
                await _user_ReviewsService.AddAsync(user_Reviews);
                // Map back to DTO and return
                var returnReview = _mapper.Map<ReviewCommand>(reviewMapper);
                return Created(returnReview);
            }
            else
                return BadRequest<ReviewCommand>();
        }

        public async Task<Response<ReviewCommand>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            //Check if the id is exist or not
            var review = await _reviewService.GetReviewByIdAsyncWithInclude(request.Id);
            //Return NotFound
            if (review == null) return NotFound<ReviewCommand>();

            if (review.UserReviews.Any(ur => ur.ApplicationUserId != currentUserId))
                return Unauthorized<ReviewCommand>(_localizer[SharedResourcesKeys.UnAuthorized]);

            //Mapping between request and Review
            var reviewMapper = _mapper.Map(request, review);
            //Call service that make edit
            var result = await _reviewService.EditAsync(reviewMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnReview = _mapper.Map<ReviewCommand>(reviewMapper);
                return Success(returnReview, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<ReviewCommand>();
        }

        public async Task<Response<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var review = await _reviewService.GetReviewByIdAsync(request.Id);
            //Return NotFound
            if (review == null) return NotFound<string>();
            //Call service that make delete
            var result = await _reviewService.DeleteAsync(review);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
