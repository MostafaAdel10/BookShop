using AutoMapper;
using BookShop.Core.Features.Subject.Queries.Handlers;
using BookShop.Core.Features.Subject.Queries.Models;
using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using BookShop.Core.Mapping.Subjects;
using BookShop.Core.Resources;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using MockQueryable;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.SubjectTest.Queries
{
    public class SubjectQueryHandlerTests
    {
        #region Fields
        private readonly Mock<ISubjectService> _subjectServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly IMapper _mapperMock;
        private readonly SubjectProfile _subjectProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        #endregion

        #region Constructors
        public SubjectQueryHandlerTests()
        {
            _subjectServiceMock = new();
            _bookServiceMock = new();
            _localizerMock = new();
            _subjectProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_subjectProfile));
            _mapperMock = new Mapper(configuration);
        }
        #endregion

        #region Handel Functions Test

        #region GetBooksBySubjectIdQuery Tests

        [Fact]
        public async Task Handle_GetBooksBySubjectIdQuery_Should_Return_NotFound_If_Subject_Null()
        {
            // Arrange
            var request = new GetBooksBySubjectIdQuery { Id = 1 };
            _subjectServiceMock.Setup(x => x.GetSubjectById(It.IsAny<int>())).ReturnsAsync((Subject)null!);

            var localizedString = new LocalizedString(SharedResourcesKeys.NotFound, "Not Found");
            _localizerMock.Setup(x => x[SharedResourcesKeys.NotFound]).Returns(localizedString);

            var handler = new SubjectQueryHandler(_subjectServiceMock.Object, _mapperMock, _localizerMock.Object, _bookServiceMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Not Found");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_GetBooksBySubjectIdQuery_Should_Return_Success_With_Mapped_Data()
        {
            // Arrange
            var subject = new Subject
            {
                Id = 1,
                Name = "Math",
                Name_Ar = "الرياضيات",
                SubSubjects = new List<SubSubject>
                {
                    new SubSubject { Id = 1, Name = "Algebra" ,Name_Ar = "الجبر" },
                    new SubSubject { Id = 2, Name = "Algebra 2" ,Name_Ar = "الجبر 2" }
                }
            };

            var bookList = new List<Book>
            {
                new Book { Id = 1, Title = "Algebra Book", Price = 11 },
                new Book { Id = 2, Title = "Algebra Book 2", Price = 11 }
            }.AsQueryable().BuildMock();

            _bookServiceMock.Setup(x => x.GetBookBySubjectIdQueryable(subject.Id))
                .Returns(bookList.AsQueryable());

            var paginatedList = new PaginatedResult<GetBooksListResponse>
                (new List<GetBooksListResponse> { new GetBooksListResponse(1, "Algebra Book", "Author", 10, 10, "Pub", DateTime.Now, 1, "", true) });

            _subjectServiceMock.Setup(x => x.GetSubjectById(subject.Id)).ReturnsAsync(subject);

            // Simulate ToPaginatedListAsync
            // You'll need to make a testable extension or wrap this in a service if not mockable

            var request = new GetBooksBySubjectIdQuery
            {
                Id = subject.Id,
                BookPageNumber = 1,
                BookPageSize = 10
            };

            var handler = new SubjectQueryHandler(_subjectServiceMock.Object, _mapperMock, _localizerMock.Object, _bookServiceMock.Object);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        #endregion

        #region GetSubjectByIdQuery Tests

        [Fact]
        public async Task Handle_GetSubjectByIdQuery_Should_Return_NotFound_If_Subject_Null()
        {
            // Arrange
            var request = new GetSubjectByIdQuery(1);
            _subjectServiceMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync((Subject)null!);

            var localizedString = new LocalizedString(SharedResourcesKeys.NotFound, "Not Found");
            _localizerMock.Setup(x => x[SharedResourcesKeys.NotFound]).Returns(localizedString);

            var handler = new SubjectQueryHandler(_subjectServiceMock.Object, _mapperMock, _localizerMock.Object, _bookServiceMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result.Message.Should().Be("Not Found");
        }

        [Fact]
        public async Task Handle_GetSubjectByIdQuery_Should_Return_Mapped_Subject()
        {
            // Arrange
            var request = new GetSubjectByIdQuery(1);
            var subject = new Subject { Id = 1, Name = "Science", Name_Ar = "علوم" };

            _subjectServiceMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(subject);

            var handler = new SubjectQueryHandler(_subjectServiceMock.Object, _mapperMock, _localizerMock.Object, _bookServiceMock.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(subject.Id);
        }

        #endregion

        #region GetSubjectListQuery Tests

        [Fact]
        public async Task Handle_GetSubjectListQuery_Should_Return_SubjectList()
        {
            // Arrange
            var subjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Math" , Name_Ar = "الرياضيات"},
                new Subject { Id = 2, Name = "Science" , Name_Ar = "علوم"}
            };

            var responseList = new List<GetSubjectListResponse>
            {
                new GetSubjectListResponse { Id = 1, Name = "Math" },
                new GetSubjectListResponse { Id = 2, Name = "Science" }
            };

            _subjectServiceMock.Setup(x => x.GetSubjectsListAsync()).ReturnsAsync(subjects);

            var handler = new SubjectQueryHandler(_subjectServiceMock.Object, _mapperMock, _localizerMock.Object, _bookServiceMock.Object);

            // Act
            var result = await handler.Handle(new GetSubjectListQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            result.Meta.Should().NotBeNull();
        }

        #endregion

        #endregion

    }
}
