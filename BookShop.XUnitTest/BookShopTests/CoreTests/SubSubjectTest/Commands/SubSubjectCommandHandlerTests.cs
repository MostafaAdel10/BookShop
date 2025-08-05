using AutoMapper;
using BookShop.Core.Features.SubSubject.Commands.Handlers;
using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.Core.Mapping.SubSubjects;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.SubSubjectTest.Commands
{
    public class SubSubjectCommandHandlerTests
    {
        private readonly Mock<ISubSubjectService> _subSubjectServiceMock;
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly IMapper _mapper;
        private readonly SubSubjectProfile _subSubjectProfile;

        private readonly SubSubjectCommandHandler _handler;

        public SubSubjectCommandHandlerTests()
        {
            _subSubjectServiceMock = new();
            _bookServiceMock = new();
            _localizerMock = new();
            _subSubjectProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_subSubjectProfile));
            _mapper = new Mapper(configuration);

            _handler = new SubSubjectCommandHandler(
                _subSubjectServiceMock.Object,
                _mapper,
                _localizerMock.Object,
                _bookServiceMock.Object
            );
        }

        #region AddSubSubjectCommand

        [Fact]
        public async Task Handle_AddSubSubject_Should_Return_Created_When_Success()
        {
            // Arrange
            var command = new AddSubSubjectCommand();

            _subSubjectServiceMock.Setup(s => s.AddAsync(It.IsAny<SubSubject>())).ReturnsAsync("Success");

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_AddSubSubject_Should_Return_BadRequest_When_Failed()
        {
            var command = new AddSubSubjectCommand();

            _subSubjectServiceMock.Setup(s => s.AddAsync(It.IsAny<SubSubject>())).ReturnsAsync("Failed");

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region EditSubSubjectCommand

        [Fact]
        public async Task Handle_EditSubSubject_Should_Return_Success_When_Updated()
        {
            var command = new EditSubSubjectCommand { Id = 1 };
            var entity = new SubSubject { Id = 1 };
            var dto = new SubSubjectCommand();

            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(entity);
            _subSubjectServiceMock.Setup(s => s.EditAsync(entity)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_EditSubSubject_Should_Return_NotFound_When_Entity_Does_Not_Exist()
        {
            var command = new EditSubSubjectCommand { Id = 1 };
            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync((SubSubject)null!);

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditSubSubject_Should_Return_BadRequest_When_Update_Fails()
        {
            var command = new EditSubSubjectCommand { Id = 1 };
            var entity = new SubSubject { Id = 1 };

            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(entity);
            _subSubjectServiceMock.Setup(s => s.EditAsync(entity)).ReturnsAsync("Failed");

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region DeleteSubSubjectCommand

        [Fact]
        public async Task Handle_DeleteSubSubject_Should_Return_Deleted_When_Success()
        {
            var command = new DeleteSubSubjectCommand(1);
            var entity = new SubSubject { Id = 1 };

            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(entity);
            _bookServiceMock.Setup(s => s.SubSubjectRelatedWithBook(entity.Id)).ReturnsAsync(false);
            _subSubjectServiceMock.Setup(s => s.DeleteAsync(entity)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_DeleteSubSubject_Should_Return_NotFound_When_Entity_Does_Not_Exist()
        {
            var command = new DeleteSubSubjectCommand(1);
            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync((SubSubject)null!);

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteSubSubject_Should_Return_Unprocessable_When_Related_With_Books()
        {
            var command = new DeleteSubSubjectCommand(1);
            var entity = new SubSubject { Id = 1 };

            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(entity);
            _bookServiceMock.Setup(s => s.SubSubjectRelatedWithBook(entity.Id)).ReturnsAsync(true);

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableContent);
        }

        [Fact]
        public async Task Handle_DeleteSubSubject_Should_Return_BadRequest_When_Delete_Fails()
        {
            var command = new DeleteSubSubjectCommand(1);
            var entity = new SubSubject { Id = 1 };

            _subSubjectServiceMock.Setup(s => s.GetByIdAsync(command.Id)).ReturnsAsync(entity);
            _bookServiceMock.Setup(s => s.SubSubjectRelatedWithBook(entity.Id)).ReturnsAsync(false);
            _subSubjectServiceMock.Setup(s => s.DeleteAsync(entity)).ReturnsAsync("Failed");

            var result = await _handler.Handle(command, default);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}
