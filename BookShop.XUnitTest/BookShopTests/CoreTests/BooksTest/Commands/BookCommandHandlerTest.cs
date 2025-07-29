using AutoMapper;
using BookShop.Core.Features.Books.Commands.Handlers;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Mapping.Books;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using BookShop.XUnitTest.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.BooksTest.Commands
{
    public class BookCommandHandlerTest
    {
        #region Fields
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly Mock<IBook_DiscountService> _bookDiscountServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<ICartItemService> _cartItemServiceMock;
        private readonly Mock<IReviewService> _reviewServiceMock;
        private readonly Mock<IBook_ImageService> _bookImageServiceMock;
        private readonly IMapper _mapper;
        private readonly BookProfile _bookProfile;
        #endregion

        #region Constructors
        public BookCommandHandlerTest()
        {
            _bookServiceMock = new();
            _currentUserServiceMock = new();
            _discountServiceMock = new();
            _bookDiscountServiceMock = new();
            _fileServiceMock = new();
            _localizerMock = new();
            _cartItemServiceMock = new();
            _reviewServiceMock = new();
            _bookImageServiceMock = new();
            _bookProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_bookProfile));
            _mapper = new Mapper(configuration);
        }
        #endregion


        #region Handel Functions Test

        #region AddBookCommand Tests
        [Fact]
        public async Task Handle_AddBookCommand_Should_Return_Created_When_Valid()
        {
            // Arrange
            var ImageData = TestFileHelper.CreateTestFormFile();
            var command = new AddBookCommand
            {
                Title = "Test Book",
                Price = 100,
                Author = "Author",
                ImageData = ImageData,
                Discounts = new List<int> { 1 },
                SubjectId = 1,
                SubSubjectId = 1,
                PublicationDate = DateTime.UtcNow,
                Unit_Instock = 5,
                IsActive = true
            };

            var discount = new Discount { Id = 1, IsActive = true, Percentage = 10 };
            var book = new Book { Id = 5, Title = "Test Book" };

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);
            _discountServiceMock.Setup(s => s.GetDiscountByIdAsync(1)).ReturnsAsync(discount);
            _fileServiceMock.Setup(f => f.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("/images/Books/test.jpg");
            _bookServiceMock.Setup(s => s.AddAsyncReturnId(It.IsAny<Book>())).ReturnsAsync(book);

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            _bookServiceMock.Verify(x => x.AddAsyncReturnId(It.IsAny<Book>()), Times.Once, "Not Called");
        }
        [Fact]
        public async Task Handle_AddBookCommand_Should_Return_NotFound_When_DiscountDoesNotExist()
        {
            var command = new AddBookCommand
            {
                Title = "Book",
                Price = 100,
                Discounts = new List<int> { 1 },
            };

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);
            _discountServiceMock.Setup(d => d.GetDiscountByIdAsync(1)).ReturnsAsync((Discount)null!);

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Handle_AddBookCommand_Should_Return_BadRequest_When_UploadFails()
        {
            // Arrange
            var ImageData = TestFileHelper.CreateTestFormFile();
            var command = new AddBookCommand
            {
                Title = "Book",
                Price = 100,
                Author = "Author",
                ImageData = ImageData,
                Discounts = new List<int>()
            };

            var book = new Book { Id = 1 };

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);
            _fileServiceMock.Setup(f => f.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync((string)null!);

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region AddImagesCommand Tests

        [Fact]
        public async Task Handle_AddImagesCommand_ReturnsCreated_WhenUploadSucceeds()
        {
            // Arrange
            var book = new Book { Id = 1 };

            var files = new List<IFormFile>
            {
                TestFileHelper.CreateTestFormFile("file content", "image1.jpg"),
                TestFileHelper.CreateTestFormFile("file content", "image2.jpg")
            };

            var command = new AddImagesCommand
            {
                BookId = 1,
                Images = files
            };

            _bookServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(book);
            _fileServiceMock.Setup(f => f.UploadImagesAsync(It.IsAny<IEnumerable<IFormFile>>(), "Books"))
                           .ReturnsAsync(new List<string> { "/images/Books/img1.jpg", "/images/Books/img2.jpg" });

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_AddImagesCommand_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var command = new AddImagesCommand { BookId = 1, Images = new List<IFormFile>() };

            _bookServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Book)null!);

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_AddImagesCommand_ReturnsBadRequest_WhenImagesNullOrEmpty()
        {
            var book = new Book { Id = 1 };

            _bookServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(book);

            var command = new AddImagesCommand { BookId = 1, Images = new List<IFormFile>() };

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_AddImagesCommand_ReturnsBadRequest_WhenUploadThrowsException()
        {
            var book = new Book { Id = 1 };

            var images = new List<IFormFile> { TestFileHelper.CreateTestFormFile() };

            _bookServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(book);
            _fileServiceMock.Setup(f => f.UploadImagesAsync(It.IsAny<IEnumerable<IFormFile>>(), "Books"))
                           .ThrowsAsync(new Exception("Failed to upload image."));

            _localizerMock.Setup(l => l[SharedResourcesKeys.FailedToUploadImage])
                         .Returns(new LocalizedString(nameof(SharedResourcesKeys.FailedToUploadImage), "Failed to upload image."));

            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            var command = new AddImagesCommand { BookId = 1, Images = images };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Message.Should().Be("Failed to upload image.");
        }

        #endregion

        #region EditBookCommand Tests

        [Fact]
        public async Task Handle_EditBookCommand_Should_ReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var request = new EditBookCommand { Id = 1 };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(request.Id))
                            .ReturnsAsync((Book)null!);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditBookCommand_Should_Return_BadRequest_WhenImageUploadFails()
        {
            // Arrange
            var book = new Book { Id = 1, Image_url = "old.jpg" };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _fileServiceMock.Setup(x => x.UpdateImageAsync(It.IsAny<string>(), It.IsAny<IFormFile>(),
                It.IsAny<string>())).ReturnsAsync((string)null!);

            var command = new EditBookCommand { Id = 1, ImageData = Mock.Of<IFormFile>() };

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_EditBookCommand_Should_Return_UnprocessableEntity_WhenDiscountIsInactive()
        {
            // Arrange
            var book = new Book { Id = 1 };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>())).ReturnsAsync(new Discount { IsActive = false });

            var command = new EditBookCommand { Id = 1, Discounts = new List<int> { 99 }, Price = 100 };

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_EditBookCommand_Should_Return_NotFound_WhenDiscountNotExist()
        {
            // Arrange
            var book = new Book { Id = 1 };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>())).ReturnsAsync((Discount)null!);

            var command = new EditBookCommand { Id = 1, Discounts = new List<int> { 99 }, Price = 100 };

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditBookCommand_Should_Return_Success_WhenEditSuccessful()
        {
            // Arrange
            var book = new Book { Id = 1, Discount = new List<Book_Discount>() };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>())).ReturnsAsync(new Discount { IsActive = true, Percentage = 10 });

            _bookServiceMock.Setup(x => x.EditAsync(It.IsAny<Book>())).ReturnsAsync("Success");

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(123);

            var command = new EditBookCommand { Id = 1, Discounts = new List<int> { 1 }, Price = 100, Title = "T", Description = "D", Author = "A", PublicationDate = DateTime.Now };

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_EditBookCommand_Should_Return_BadRequest_WhenEditFails()
        {
            // Arrange
            var book = new Book { Id = 1, Discount = new List<Book_Discount>() };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Discount { IsActive = true, Percentage = 10 });

            _bookServiceMock.Setup(x => x.EditAsync(It.IsAny<Book>())).ReturnsAsync("Fail");

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(123);

            var command = new EditBookCommand { Id = 1, Discounts = new List<int> { 1 }, Price = 100, Title = "T", Description = "D", Author = "A", PublicationDate = DateTime.Now };

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region DeleteBookCommand Tests

        [Fact]
        public async Task Handle_DeleteBookCommand_Should_ReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null!);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteBookCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteBookCommand_Should_Return_UnprocessableEntity_WhenBookRelatedToCartItem()
        {
            // Arrange
            var book = new Book { Id = 1 };

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            _cartItemServiceMock.Setup(x => x.IsBookRelatedWithCartItem(book.Id)).ReturnsAsync(true);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteBookCommand(book.Id), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_DeleteBookCommand_Should_Return_UnprocessableEntity_WhenBookRelatedToReview()
        {
            // Arrange
            var book = new Book { Id = 1 };

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            _cartItemServiceMock.Setup(x => x.IsBookRelatedWithCartItem(book.Id)).ReturnsAsync(false);
            _reviewServiceMock.Setup(x => x.IsBookRelatedWithReview(book.Id)).ReturnsAsync(true);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteBookCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_DeleteBookCommand_Should_Return_BadRequest_WhenBookHasImageButDeleteImageFailed()
        {
            // Arrange
            var bookWithImage = new Book { Id = 1, Image_url = "/images/Books/test.jpg" };

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(bookWithImage);
            _cartItemServiceMock.Setup(x => x.IsBookRelatedWithCartItem(It.IsAny<int>())).ReturnsAsync(false);
            _reviewServiceMock.Setup(x => x.IsBookRelatedWithReview(It.IsAny<int>())).ReturnsAsync(false);
            _fileServiceMock.Setup(x => x.DeleteImage(bookWithImage.Image_url)).Returns(false);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteBookCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_DeleteBookCommand_Should_Return_Success_WhenFullSuccessfulDeletion()
        {
            // Arrange
            var book = new Book { Id = 1, Image_url = "/images/Books/test.jpg" };

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            _cartItemServiceMock.Setup(x => x.IsBookRelatedWithCartItem(book.Id)).ReturnsAsync(false);
            _reviewServiceMock.Setup(x => x.IsBookRelatedWithReview(book.Id)).ReturnsAsync(false);
            _fileServiceMock.Setup(x => x.DeleteImage(book.Image_url)).Returns(true);
            _bookImageServiceMock.Setup(x => x.GetBook_ImagesByBookIdAsync(book.Id)).ReturnsAsync(new List<Book_Image>());
            _bookDiscountServiceMock.Setup(x => x.GetBook_DiscountsByBookIdAsync(book.Id)).ReturnsAsync(new List<Book_Discount>());
            _bookServiceMock.Setup(x => x.DeleteAsync(book)).ReturnsAsync("Success");

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteBookCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_DeleteBookCommand_Should_Return_BadRequest_WhenDeletionFailed_ServiceReturnedNotSuccess()
        {
            // Arrange
            var book = new Book { Id = 1, Image_url = "/images/Books/test.jpg" };

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            _cartItemServiceMock.Setup(x => x.IsBookRelatedWithCartItem(book.Id)).ReturnsAsync(false);
            _reviewServiceMock.Setup(x => x.IsBookRelatedWithReview(book.Id)).ReturnsAsync(false);
            _fileServiceMock.Setup(x => x.DeleteImage(book.Image_url)).Returns(true);
            _bookImageServiceMock.Setup(x => x.GetBook_ImagesByBookIdAsync(book.Id)).ReturnsAsync(new List<Book_Image>());
            _bookDiscountServiceMock.Setup(x => x.GetBook_DiscountsByBookIdAsync(book.Id)).ReturnsAsync(new List<Book_Discount>());
            _bookServiceMock.Setup(x => x.DeleteAsync(book)).ReturnsAsync("Failed");

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteBookCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region DeleteImageFromBookCommand Test

        [Fact]
        public async Task Handle_DeleteImageFromBookCommand_Should_ReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(It.IsAny<int>()))
                .ReturnsAsync((Book)null!);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteImageFromBookCommand { BookId = 1, ImageUrl = "/img.png" }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteImageFromBookCommand_Should_ReturnNotFound_WhenImageNotFoundInBook()
        {
            // Arrange
            var book = new Book { Id = 1 };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(It.IsAny<int>()))
                .ReturnsAsync(book);
            _bookImageServiceMock.Setup(x => x.GetImageByBookIdAndImageUrlAsync(book.Id, "/img.png"))
                                  .ReturnsAsync((Book_Image)null!);


            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteImageFromBookCommand { BookId = book.Id, ImageUrl = "/img.png" }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteImageFromBookCommand_Should_ReturnBadRequest_WhenFileServiceFailedToDeleteImage()
        {
            // Arrange
            var book = new Book { Id = 1 };
            var bookImage = new Book_Image { Image_url = "/img.png" };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _bookImageServiceMock.Setup(x => x.GetImageByBookIdAndImageUrlAsync(book.Id, "/img.png"))
                                  .ReturnsAsync(bookImage);

            _fileServiceMock.Setup(x => x.DeleteImage(bookImage.Image_url)).Returns(false);


            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteImageFromBookCommand { BookId = book.Id, ImageUrl = "/img.png" }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_DeleteImageFromBookCommand_Should_ReturnSuccess_WhenSuccessfulDeletion()
        {
            // Arrange
            var book = new Book { Id = 1 };
            var bookImage = new Book_Image { Id = 1, Image_url = "/img.png" };

            _bookServiceMock.Setup(x => x.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);

            _bookImageServiceMock.Setup(x => x.GetImageByBookIdAndImageUrlAsync(book.Id, "/img.png"))
                                  .ReturnsAsync(bookImage);

            _fileServiceMock.Setup(x => x.DeleteImage(bookImage.Image_url)).Returns(true);

            _bookImageServiceMock.Setup(x => x.DeleteAsync(bookImage)).ReturnsAsync("Success");



            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteImageFromBookCommand { BookId = book.Id, ImageUrl = "/img.png" }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        #endregion

        #region DeleteDiscountFromBooksCommand Test

        [Fact]
        public async Task Handle_DeleteDiscountFromBooksCommand_Should_ReturnNotFound_WhenBookDiscountListEmptyOrNull()
        {
            // Arrange
            _bookDiscountServiceMock.Setup(x => x.GetBook_DiscountsByDiscountIdAsync(It.IsAny<int>()))
                         .ReturnsAsync((List<Book_Discount>)null!);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteDiscountFromBooksCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteDiscountFromBooksCommand_Should_ReturnNotFound_WhenDiscountNotFound()
        {
            // Arrange
            var bookDiscount = new Book_Discount { BookId = 1, DiscountId = 1 };

            _bookDiscountServiceMock.Setup(x => x.GetBook_DiscountsByDiscountIdAsync(It.IsAny<int>()))
                         .ReturnsAsync(new List<Book_Discount> { bookDiscount });

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Discount)null!);

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteDiscountFromBooksCommand(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteDiscountFromBooksCommand_Should_ReturnNotFound_WhenBookIsNull()
        {
            // Arrange
            var bookDiscount = new Book_Discount { BookId = 1, DiscountId = 1 };
            var discount = new Discount { Id = 1, Code = 10, IsActive = true, Name = "dis", Name_Ar = "خصم", Percentage = 10 };

            _bookDiscountServiceMock.Setup(x => x.GetBook_DiscountsByDiscountIdAsync(It.IsAny<int>()))
                         .ReturnsAsync(new List<Book_Discount> { bookDiscount });

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(discount);

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Book)null!); // simulate missing book

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteDiscountFromBooksCommand(1), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_DeleteDiscountFromBooksCommand_Should_ReturnSuccessful_WhenDiscountIsDeleted()
        {
            // Arrange
            var bookDiscount = new Book_Discount { BookId = 1, DiscountId = 1 };
            var book = new Book { Id = 1, Title = "Test Book" };
            var discount = new Discount { Id = 1, Code = 10, IsActive = true, Name = "dis", Name_Ar = "خصم", Percentage = 10 };

            _bookDiscountServiceMock.Setup(x => x.GetBook_DiscountsByDiscountIdAsync(It.IsAny<int>()))
                          .ReturnsAsync(new List<Book_Discount> { bookDiscount });

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(discount);

            _bookServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(book);

            _bookServiceMock.Setup(x => x.EditAsync(book)).ReturnsAsync("Success");

            _bookDiscountServiceMock.Setup(x => x.DeleteBookDiscountAsync(It.IsAny<Book_Discount>()))
                         .ReturnsAsync("Success");

            // inject mocks
            var handler = new BookCommandHandler(
                _bookServiceMock.Object,
                _mapper,
                _cartItemServiceMock.Object,
                _reviewServiceMock.Object,
                _fileServiceMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _bookImageServiceMock.Object,
                _bookDiscountServiceMock.Object,
                _discountServiceMock.Object
            );

            // Act
            var result = await handler.Handle(new DeleteDiscountFromBooksCommand(1), CancellationToken.None);


            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #endregion
    }
}
