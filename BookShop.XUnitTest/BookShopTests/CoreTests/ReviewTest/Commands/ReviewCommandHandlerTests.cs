using AutoMapper;
using BookShop.Core.Features.Review.Commands.Handlers;
using BookShop.Core.Features.Review.Commands.Models;
using BookShop.Core.Mapping.Review;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.ReviewTest.Commands
{
    public class ReviewCommandHandlerTests
    {
        #region Fields
        private readonly Mock<IReviewService> _reviewServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IUser_ReviewsService> _userReviewServiceMock;
        private readonly IMapper _mapper;
        private readonly ReviewProfile _reviewProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;

        private readonly ReviewCommandHandler _handler;
        #endregion

        #region Constructors
        public ReviewCommandHandlerTests()
        {
            _reviewServiceMock = new();
            _bookServiceMock = new();
            _currentUserServiceMock = new();
            _userReviewServiceMock = new();
            _localizerMock = new();
            _reviewProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_reviewProfile));
            _mapper = new Mapper(configuration);

            _handler = new ReviewCommandHandler(
                _reviewServiceMock.Object,
                _mapper,
                _currentUserServiceMock.Object,
                _bookServiceMock.Object,
                _userReviewServiceMock.Object,
                _localizerMock.Object
            );
        }
        #endregion

        #region Handel Functions Test

        #region AddReviewCommand
        [Fact]
        public async Task Handle_AddReviewCommand_Should_Return_Created_When_Success()
        {
            // Arrange
            var command = new AddReviewCommand { BookId = 1 };
            var reviewEntity = new Review();
            var savedReview = new Review { Id = 10 };

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(123);
            _bookServiceMock.Setup(x => x.GetByIdAsync(command.BookId)).ReturnsAsync(new Book());
            _reviewServiceMock.Setup(x => x.AddAsyncWithReturnId(It.IsAny<Review>())).ReturnsAsync(savedReview);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_AddReviewCommand_Should_Return_NotFound_When_Book_Not_Found()
        {
            var command = new AddReviewCommand { BookId = 100 };
            _bookServiceMock.Setup(x => x.GetByIdAsync(command.BookId)).ReturnsAsync((Book)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_AddReviewCommand_Should_Return_BadRequest_When_Failed()
        {
            var command = new AddReviewCommand { BookId = 1 };
            var reviewEntity = new Review();

            _bookServiceMock.Setup(x => x.GetByIdAsync(command.BookId)).ReturnsAsync(new Book());
            _reviewServiceMock.Setup(x => x.AddAsyncWithReturnId(reviewEntity)).ReturnsAsync((Review)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region EditReviewCommand
        [Fact]
        public async Task Handle_EditReviewCommand_Should_Return_Success_When_Valid()
        {
            var command = new EditReviewCommand { Id = 1 };
            var reviewEntity = new Review
            {
                UserReviews = new List<User_Reviews>
                {
                    new User_Reviews { ApplicationUserId = 123 }
                }
            };

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(123);
            _reviewServiceMock.Setup(x => x.GetReviewByIdAsyncWithInclude(command.Id)).ReturnsAsync(reviewEntity);
            _reviewServiceMock.Setup(x => x.EditAsync(reviewEntity)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_EditReviewCommand_Should_Return_NotFound_When_Review_Not_Found()
        {
            var command = new EditReviewCommand { Id = 1 };
            _reviewServiceMock.Setup(x => x.GetReviewByIdAsyncWithInclude(command.Id)).ReturnsAsync((Review)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_Should_Return_Unauthorized_When_Not_Owner()
        {
            // Arrange
            var command = new EditReviewCommand { Id = 1 };
            var review = new Review
            {
                Id = 1,
                UserReviews = new List<User_Reviews>
                {
                    new User_Reviews { ApplicationUserId = 123 }
                }
            };

            var currentUserId = 321;

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(currentUserId);

            _reviewServiceMock.Setup(x => x.GetReviewByIdAsyncWithInclude(command.Id))
                .ReturnsAsync(review);

            _localizerMock.Setup(l => l[SharedResourcesKeys.UnAuthorized])
                .Returns(new LocalizedString(SharedResourcesKeys.UnAuthorized, "Unauthorized"));

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            result.Message.Should().Be("Unauthorized");
        }

        [Fact]
        public async Task Handle_EditReviewCommand_Should_Return_BadRequest_When_Failed()
        {
            var command = new EditReviewCommand { Id = 1 };
            var reviewEntity = new Review
            {
                UserReviews = new List<User_Reviews>
                {
                    new User_Reviews { ApplicationUserId = 123 }
                }
            };

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(123);
            _reviewServiceMock.Setup(x => x.GetReviewByIdAsyncWithInclude(command.Id)).ReturnsAsync(reviewEntity);
            _reviewServiceMock.Setup(x => x.EditAsync(reviewEntity)).ReturnsAsync("Failed");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region DeleteReviewCommand
        [Fact]
        public async Task Handle_DeleteReviewCommand_Should_Return_Success_When_Deleted()
        {
            var command = new DeleteReviewCommand(1);
            var reviewEntity = new Review { Id = 1 };

            _reviewServiceMock.Setup(x => x.GetReviewByIdAsync(command.Id)).ReturnsAsync(reviewEntity);
            _reviewServiceMock.Setup(x => x.DeleteAsync(reviewEntity)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_DeleteReviewCommand_Should_Return_NotFound_When_Not_Found()
        {
            var command = new DeleteReviewCommand(1);

            _reviewServiceMock.Setup(x => x.GetReviewByIdAsync(command.Id)).ReturnsAsync((Review)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteReviewCommand_Should_Return_BadRequest_When_Failed()
        {
            var command = new DeleteReviewCommand(1);
            var reviewEntity = new Review { Id = 1 };

            _reviewServiceMock.Setup(x => x.GetReviewByIdAsync(command.Id)).ReturnsAsync(reviewEntity);
            _reviewServiceMock.Setup(x => x.DeleteAsync(reviewEntity)).ReturnsAsync("Failed");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #endregion

    }
}
