using AutoMapper;
using BookShop.Core.Features.Subject.Commands.Handlers;
using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.Core.Mapping.Subjects;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.SubjectTest.Commands
{
    public class SubjectCommandHandlerTests
    {
        #region Fields
        private readonly Mock<ISubjectService> _subjectServiceMock;
        private readonly Mock<ISubSubjectService> _subSubjectServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;
        private readonly SubjectProfile _subjectProfile;
        private readonly SubjectCommandHandler _handler;
        #endregion

        #region Constructors
        public SubjectCommandHandlerTests()
        {
            _subjectServiceMock = new();
            _subSubjectServiceMock = new();
            _bookServiceMock = new();
            _localizerMock = new();
            _subjectProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_subjectProfile));
            _mapper = new Mapper(configuration);

            _handler = new SubjectCommandHandler(
                _subjectServiceMock.Object,
                _mapper,
                _localizerMock.Object,
                _subSubjectServiceMock.Object,
                _bookServiceMock.Object
            );
        }
        #endregion

        #region Handel Functions Test

        #region AddSubjectCommand

        [Fact]
        public async Task Handle_AddSubjectCommand_Success()
        {
            // Arrange
            var command = new AddSubjectCommand { Name = "Math", Name_Ar = "الرياضيات" };

            _subjectServiceMock.Setup(s => s.AddAsync(It.IsAny<Subject>())).ReturnsAsync("Success");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_AddSubjectCommand_Failure()
        {
            var command = new AddSubjectCommand();
            var subjectEntity = new Subject();

            _subjectServiceMock.Setup(s => s.AddAsync(subjectEntity)).ReturnsAsync("Error");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
        }

        #endregion

        #region EditSubjectCommand

        [Fact]
        public async Task Handle_EditSubjectCommand_Success()
        {
            var command = new EditSubjectCommand { Id = 1 };
            var subjectEntity = new Subject();

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(subjectEntity);
            _subjectServiceMock.Setup(s => s.EditAsync(It.IsAny<Subject>())).ReturnsAsync("Success");
            _localizerMock.Setup(l => l[SharedResourcesKeys.Updated]).Returns(new LocalizedString(SharedResourcesKeys.Updated, "Updated"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_EditSubjectCommand_NotFound()
        {
            var command = new EditSubjectCommand { Id = 1 };

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync((Subject)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditSubjectCommand_Failure()
        {
            var command = new EditSubjectCommand { Id = 1 };
            var subjectEntity = new Subject();

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(subjectEntity);
            _subjectServiceMock.Setup(s => s.EditAsync(subjectEntity)).ReturnsAsync("Error");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region DeleteSubjectCommand

        [Fact]
        public async Task Handle_DeleteSubjectCommand_Success()
        {
            var command = new DeleteSubjectCommand(1);
            var subjectEntity = new Subject { Id = 1 };

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(subjectEntity);
            _subSubjectServiceMock.Setup(s => s.SubjectRelatedWithSubSubject(subjectEntity.Id)).ReturnsAsync(false);
            _bookServiceMock.Setup(s => s.SubjectRelatedWithBook(subjectEntity.Id)).ReturnsAsync(false);
            _subjectServiceMock.Setup(s => s.DeleteAsync(subjectEntity)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_DeleteSubjectCommand_NotFound()
        {
            var command = new DeleteSubjectCommand(1);

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync((Subject)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteSubjectCommand_RelatedWithSubSubject_ThrowsException()
        {
            var command = new DeleteSubjectCommand(1);
            var subjectEntity = new Subject { Id = 1 };

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(subjectEntity);
            _subSubjectServiceMock.Setup(s => s.SubjectRelatedWithSubSubject(subjectEntity.Id)).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_DeleteSubjectCommand_RelatedWithBook_ThrowsException()
        {
            var command = new DeleteSubjectCommand(1);
            var subjectEntity = new Subject { Id = 1 };

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(subjectEntity);
            _subSubjectServiceMock.Setup(s => s.SubjectRelatedWithSubSubject(subjectEntity.Id)).ReturnsAsync(false);
            _bookServiceMock.Setup(s => s.SubjectRelatedWithBook(subjectEntity.Id)).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_DeleteSubjectCommand_Failure()
        {
            var command = new DeleteSubjectCommand(1);
            var subjectEntity = new Subject { Id = 1 };

            _subjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(subjectEntity);
            _subSubjectServiceMock.Setup(s => s.SubjectRelatedWithSubSubject(subjectEntity.Id)).ReturnsAsync(false);
            _bookServiceMock.Setup(s => s.SubjectRelatedWithBook(subjectEntity.Id)).ReturnsAsync(false);
            _subjectServiceMock.Setup(s => s.DeleteAsync(subjectEntity)).ReturnsAsync("Error");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
        }

        #endregion

        #endregion

    }
}
