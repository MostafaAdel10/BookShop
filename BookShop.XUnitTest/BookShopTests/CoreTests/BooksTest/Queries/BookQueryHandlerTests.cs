using AutoMapper;
using BookShop.Core.Features.Books.Queries.Handlers;
using BookShop.Core.Features.Books.Queries.Models;
using BookShop.Core.Features.Books.Queries.Response_DTO_;
using BookShop.Core.Mapping.Books;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Enums;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using MockQueryable;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.BooksTest.Queries
{
    public class BookQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly IMapper _mapperMock;
        private readonly BookProfile _bookProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        #endregion

        #region Constructors
        public BookQueryHandlerTests()
        {
            _bookServiceMock = new();
            _localizerMock = new();
            _bookProfile = new();
            var configuration = new MapperConfiguration(c => c.AddProfile(_bookProfile));
            _mapperMock = new Mapper(configuration);
        }
        #endregion


        #region Handel Functions Test

        #region GetBookListQuery Tests

        [Fact]
        public async Task Handle_GetBookListQuery_ReturnsList_WhenBooksExist()
        {
            //Arrange
            var booksList = new List<Book> { new Book { Id = 1, Title = "Clean Code", Author = "Robert" } };

            _bookServiceMock.Setup(s => s.GetBooksListAsync()).Returns(Task.FromResult(booksList));
            var handler = new BookQueryHandler(_bookServiceMock.Object, _mapperMock, _localizerMock.Object);

            //Act
            var result = await handler.Handle(new GetBookListQuery(), CancellationToken.None);

            //Assert
            Assert.True(result.Succeeded);
            Assert.Single(result.Data);
        }
        [Fact]
        public async Task Handle_GetBookListQuery_Should_NotNull_And_NotEmpty()
        {
            //Arrange
            var booksList = new List<Book> { new Book { Id = 1, Title = "Clean Code", Author = "Robert" } };

            var query = new GetBookListQuery();

            _bookServiceMock.Setup(s => s.GetBooksListAsync()).Returns(Task.FromResult(booksList));

            var handler = new BookQueryHandler(_bookServiceMock.Object, _mapperMock, _localizerMock.Object);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Data.Should().NotBeNullOrEmpty();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeOfType<List<GetBookListResponse>>();
            Assert.Equal(1, result.Data.Count);
        }
        [Fact]
        public async Task Handle_GetBookListQuery_ReturnsEmptyList_WhenNoBooksExist()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.GetBooksListAsync()).ReturnsAsync(new List<Book>());
            var handler = new BookQueryHandler(_bookServiceMock.Object, _mapperMock, _localizerMock.Object);

            // Act
            var result = await handler.Handle(new GetBookListQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEmpty();
        }

        #endregion

        #region GetBookByIdQuery Tests

        [Fact]
        public async Task Handle_GetBookByIdQuery_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Domain-Driven Design",
                Author = "Eric Evans",
                Images = new List<Book_Image>
                {
                    new Book_Image { Image_url = "img1.jpg" },
                    new Book_Image { Image_url = "img2.jpg" }
                }
            };

            _bookServiceMock.Setup(s => s.GetBookByIdWithIncludeAsync(book.Id)).ReturnsAsync(book);
            var handler = new BookQueryHandler(_bookServiceMock.Object, _mapperMock, _localizerMock.Object);

            var query = new GetBookByIdQuery(book.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(book.Id);
            result.Data.Images.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_GetBookByIdQuery_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var id = 999;
            _bookServiceMock.Setup(s => s.GetBookByIdWithIncludeAsync(id)).ReturnsAsync((Book)null!);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.NotFound])
                .Returns(new LocalizedString(SharedResourcesKeys.NotFound, "Not Found"));

            var handler = new BookQueryHandler(_bookServiceMock.Object, _mapperMock, _localizerMock.Object);

            var query = new GetBookByIdQuery(id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result.Message.Should().Be("Not Found");
        }

        #endregion

        #region GetBookPaginatedListQuery Tests

        [Fact]
        public async Task Handle_GetBookPaginatedListQuery_ReturnsPaginatedResult_WhenBooksExist()
        {
            // Arrange
            var request = new GetBookPaginatedListQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Search = "",
                OrderBy = BookOrderingEnum.Title
            };

            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Refactoring",
                    Description = "Improving the Design of Existing Code",
                    ISBN13 = "1234567890123",
                    ISBN10 = "1234567890",
                    Author = "Martin Fowler",
                    Price = 50,
                    PriceAfterDiscount = 45,
                    Publisher = "Addison-Wesley",
                    PublicationDate = new DateTime(2018, 1, 1),
                    Unit_Instock = 10,
                    Image_url = "img1.jpg",
                    IsActive = true,
                    Subject = new Subject { Name = "Software", Name_Ar = "البرمجيات" },
                    SubSubject = new SubSubject { Name = "Refactoring", Name_Ar = "تحسين الكود" }
                }
            }.AsQueryable().BuildMock(); // 🔥 هذا يحل المشكلة

            _bookServiceMock
                .Setup(s => s.FilterBookPaginatedQueryable(request.OrderBy, request.Search))
                .Returns(books.AsQueryable());

            var handler = new BookQueryHandler(_bookServiceMock.Object, _mapperMock, _localizerMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
            result.Meta.Should().NotBeNull();
            result.Data.First().Title.Should().Be("Refactoring");
        }

        #endregion


        #endregion
    }
}
