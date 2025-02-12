using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
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
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ReviewCommandHandler(IReviewService reviewService, IMapper mapper,
            IBookService bookService, IApplicationUserService applicationUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _bookService = bookService;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<ReviewCommand>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            // Validate Product and User existence
            var book = await _bookService.GetByIdAsync(request.BookId);
            var user = await _applicationUserService.GetByIdAsync(request.UserId);

            if (book == null || user == null) return NotFound<ReviewCommand>(_localizer[SharedResourcesKeys.InvalidFileType]);

            //Mapping between request and Review
            var reviewMapper = _mapper.Map<DataAccess.Entities.Review>(request);

            reviewMapper.Book = book;
            reviewMapper.UserReviews = new List<User_Reviews> { new User_Reviews { applicationUser = user, review = reviewMapper } };
            //Add
            var result = await _reviewService.AddAsync(reviewMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnReview = _mapper.Map<ReviewCommand>(reviewMapper);
                return Created(returnReview);
            }
            else
                return BadRequest<ReviewCommand>();
        }

        public async Task<Response<ReviewCommand>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var review = await _reviewService.GetReviewByIdAsync(request.Id);
            //Return NotFound
            if (review == null) return NotFound<ReviewCommand>();
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
