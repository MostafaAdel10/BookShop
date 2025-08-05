using AutoMapper;
using BookShop.Core.Features.SubSubject.Queries.Handlers;
using BookShop.Core.Features.SubSubject.Queries.Models;
using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;
using BookShop.Core.Mapping.SubSubjects;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using MockQueryable;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.SubSubjectTest.Queries
{
    public class SubSubjectQueryHandlerTests
    {
        #region Fields
        private readonly Mock<ISubSubjectService> _subSubjectServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly IMapper _mapper;
        private readonly SubSubjectProfile _subSubjectProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly SubSubjectQueryHandler _handler;
        #endregion

        #region Constructors
        public SubSubjectQueryHandlerTests()
        {
            _subSubjectServiceMock = new();
            _bookServiceMock = new();
            _localizerMock = new();
            _subSubjectProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_subSubjectProfile));
            _mapper = new Mapper(configuration);

            _handler = new SubSubjectQueryHandler(
            _subSubjectServiceMock.Object,
            _mapper,
            _localizerMock.Object,
            _bookServiceMock.Object);
        }
        #endregion

        #region Handel Functions Test

        #region GetSubSubjectList
        [Fact]
        public async Task GetSubSubjectList_ShouldReturnList()
        {
            // Arrange
            var subSubjects = new List<SubSubject> { new() { Id = 1, Name = "Sub1" } };
            var mapped = new List<GetSubSubjectListResponses> { new() { Id = 1, Name = "Sub1" } };

            _subSubjectServiceMock.Setup(x => x.GetSubSubjectsListAsync()).ReturnsAsync(subSubjects);

            // Act
            var result = await _handler.Handle(new GetSubSubjectListQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(mapped);
            result.Meta.Should().NotBeNull();
        }
        #endregion

        #region GetSubSubjectById
        [Fact]
        public async Task GetSubSubjectById_ShouldReturnItem_WhenFound()
        {
            // Arrange
            var subSubject = new SubSubject { Id = 1, Name = "Test" };

            _subSubjectServiceMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(subSubject);

            // Act
            var result = await _handler.Handle(new GetSubSubjectByIdQuery(1), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Should().NotBeNull();
            result.Data.Id.Should().Be(subSubject.Id);
        }

        [Fact]
        public async Task GetSubSubjectById_ShouldReturnNotFound_WhenNotExist()
        {
            // Arrange
            _subSubjectServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((SubSubject)null!);

            var localizedString = new LocalizedString(SharedResourcesKeys.NotFound, "Not Found");
            _localizerMock.Setup(x => x[SharedResourcesKeys.NotFound]).Returns(localizedString);

            // Act
            var result = await _handler.Handle(new GetSubSubjectByIdQuery(1), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result.Message.Should().Be("Not Found");
        }
        #endregion

        #region GetBooksBySubSubjectId

        [Fact]
        public async Task GetBooksBySubSubjectId_ShouldReturnBooks_WhenSubSubjectExists()
        {
            // Arrange
            var subSubject = new SubSubject { Id = 1, Name = "Sub" };
            var books = new List<Book>
            {
                new() { Id = 1, Title = "Book1", Author = "Author1", Price = 100, PriceAfterDiscount = 80, Publisher = "Pub", PublicationDate = DateTime.Now, Unit_Instock = 5, Image_url = "url", IsActive = true }
            }.AsQueryable().BuildMock();

            _subSubjectServiceMock.Setup(x => x.GetSubSubjectById(1)).ReturnsAsync(subSubject);
            _bookServiceMock.Setup(x => x.GetBookBySubSubjectIdQueryable(1)).Returns(books);

            var projectedBooks = books.Select(b => new GetBooksListResponses(
                b.Id, b.Title, b.Author, b.Price, b.PriceAfterDiscount, b.Publisher,
                b.PublicationDate, b.Unit_Instock, b.Image_url, b.IsActive
            )).AsQueryable();

            // ممكن هنا تتأكد إن الـ projection شغال زي ما انت متوقع (optional)

            // Act
            var result = await _handler.Handle(
                new GetBooksBySubSubjectIdQuery { Id = 1, BookPageNumber = 1, BookPageSize = 10 },
                CancellationToken.None
            );

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }


        [Fact]
        public async Task GetBooksBySubSubjectId_ShouldReturnNotFound_WhenSubSubjectDoesNotExist()
        {
            // Arrange
            _subSubjectServiceMock.Setup(x => x.GetSubSubjectById(1)).ReturnsAsync((SubSubject)null!);

            // Act
            var result = await _handler.Handle(new GetBooksBySubSubjectIdQuery { Id = 1 }, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        #endregion

        #endregion
    }
}
