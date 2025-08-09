using AutoMapper;
using BookShop.Core.Features.Review.Queries.Handlers;
using BookShop.Core.Features.Review.Queries.Models;
using BookShop.Core.Features.Review.Queries.Response_DTO_;
using BookShop.Core.Mapping.Review;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Entities.Identity;
using BookShop.DataAccess.Enums;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using MockQueryable;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.ReviewTest.Queries
{
    public class ReviewQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IReviewService> _reviewServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly IMapper _mapper;
        private readonly ReviewProfile _reviewProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly ReviewQueryHandler _handler;
        #endregion

        #region Constructors
        public ReviewQueryHandlerTests()
        {
            _reviewServiceMock = new Mock<IReviewService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _reviewProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_reviewProfile));
            _mapper = new Mapper(configuration);

            _handler = new ReviewQueryHandler(
                _reviewServiceMock.Object,
                _mapper,
                _currentUserServiceMock.Object,
                _localizerMock.Object
            );
        }
        #endregion

        #region Handel Functions Test

        #region GetReviewListQuery Tests

        [Fact]
        public async Task Handle_GetReviewListQuery_ShouldReturnList()
        {
            var reviews = new List<Review> { new Review { Id = 1 } };
            var mapped = new List<GetReviewListResponse> { new GetReviewListResponse { Id = 1 } };

            _reviewServiceMock.Setup(s => s.GetReviewsListAsync()).ReturnsAsync(reviews);

            var result = await _handler.Handle(new GetReviewListQuery(), CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(1);
        }

        #endregion

        #region GetReviewByIdQuery Tests

        [Fact]
        public async Task Handle_GetReviewByIdQuery_ShouldReturnReview_WhenFound()
        {
            var review = new Review { Id = 1 };
            var mapped = new GetSingleReviewResponse { Id = 1 };

            _reviewServiceMock.Setup(s => s.GetReviewByIdAsync(1)).ReturnsAsync(review);

            var result = await _handler.Handle(new GetReviewByIdQuery(1), CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Id.Should().Be(1);
        }

        [Fact]
        public async Task Handle_GetReviewByIdQuery_ShouldReturnNotFound_WhenNotExist()
        {
            _reviewServiceMock.Setup(s => s.GetReviewByIdAsync(1)).ReturnsAsync((Review)null!);
            _localizerMock.Setup(l => l[SharedResourcesKeys.NotFound])
                .Returns(new LocalizedString("NotFound", "Not found"));

            var result = await _handler.Handle(new GetReviewByIdQuery(1), CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region GetReviewPaginatedListQuery Tests

        [Fact]
        public async Task Handle_GetReviewPaginatedListQuery_ShouldReturnPaginatedList()
        {
            var request = new GetReviewPaginatedListQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Search = "",
                OrderBy = ReviewOrderingEnum.Rating
            };

            var reviews = new List<GetReviewPaginatedListResponse>
            {
                new GetReviewPaginatedListResponse(1, 5, "content", 1)
            };

            _reviewServiceMock.Setup(s =>
                s.FilterReviewPaginatedQueryable(request.OrderBy, request.Search)
            ).Returns(new List<Review>
            {
                new Review { Id = 1, Rating = 5, Content = "content", BookId = 1 }
            }.AsQueryable().BuildMock());



            var result = await _handler.Handle(request, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region GetReviewByBookIdQuery Tests

        [Fact]
        public async Task Handle_GetReviewByBookIdQuery_ShouldReturnReviews_WhenExist()
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = 1,
                    BookId = 2,
                    Rating = 5,
                    Content = "Nice",
                    UserReviews = new List<User_Reviews>
                    {
                        new User_Reviews
                        {
                            applicationUser = new ApplicationUser { UserName = "mostafa" }
                        }
                    }
                }
            };

            _reviewServiceMock.Setup(s => s.GetReviewsListAsyncQueryble())
                .ReturnsAsync(reviews.AsQueryable());

            var result = await _handler.Handle(new GetReviewByBookIdQuery(2), CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_GetReviewByBookIdQuery_ShouldReturnNotFound_WhenNoReviews()
        {
            _reviewServiceMock.Setup(s => s.GetReviewsListAsyncQueryble())
                .ReturnsAsync(new List<Review>().AsQueryable());

            _localizerMock.Setup(l => l[SharedResourcesKeys.NoReviews])
                .Returns(new LocalizedString("NoReviews", "No reviews found"));

            var result = await _handler.Handle(new GetReviewByBookIdQuery(5), CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region GetReviewsByCurrentUser Tests

        [Fact]
        public async Task Handle_GetReviewsByCurrentUser_ShouldReturnUserReviews()
        {
            int userId = 123;
            _currentUserServiceMock.Setup(u => u.GetUserId()).Returns(userId);

            var reviews = new List<Review>
            {
                new Review
                {
                    Id = 1,
                    UserReviews = new List<User_Reviews>
                    {
                        new User_Reviews { ApplicationUserId = userId }
                    }
                }
            };

            var mapped = new List<GetReviewListResponse>
            {
                new GetReviewListResponse { Id = 1 }
            };

            _reviewServiceMock.Setup(s => s.GetReviewsListAsyncQueryble())
                .ReturnsAsync(reviews.AsQueryable());

            var result = await _handler.Handle(new GetReviewsByCurrentUserQuery(), CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(1);
        }

        #endregion

        #endregion

    }
}
