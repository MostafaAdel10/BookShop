using AutoMapper;
using BookShop.Core.Features.Shipping_Method.Commands.Handlers;
using BookShop.Core.Features.Shipping_Method.Commands.Models;
using BookShop.Core.Mapping.Shipping_Method;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.ShippingMethodTest.Commands
{
    public class Shipping_MethodCommandHandlerTests
    {
        #region Fields
        private readonly Mock<IShipping_MethodService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly Shipping_MethodProfile _shipping_MethodProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Shipping_MethodCommandHandler _handler;
        #endregion

        #region Constructors
        public Shipping_MethodCommandHandlerTests()
        {
            _serviceMock = new Mock<IShipping_MethodService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _shipping_MethodProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_shipping_MethodProfile));
            _mapper = new Mapper(configuration);

            _handler = new Shipping_MethodCommandHandler(
                _serviceMock.Object,
                _mapper,
                _localizerMock.Object
            );
        }
        #endregion

        #region Handle Functions Test

        #region AddShipping_MethodCommand Tests

        [Fact]
        public async Task Handle_AddShipping_MethodCommand_ShouldReturnCreated_WhenSuccess()
        {
            var command = new AddShipping_MethodCommand { Method_Name = "Express" };

            _serviceMock.Setup(s => s.AddAsync(It.IsAny<Shipping_Methods>())).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_AddShipping_MethodCommand_ShouldReturnBadRequest_WhenFailed()
        {
            var command = new AddShipping_MethodCommand { Method_Name = "Express" };
            var entity = new Shipping_Methods { Method_Name = "Express" };

            _serviceMock.Setup(s => s.AddAsync(entity)).ReturnsAsync("Fail");

            _localizerMock.Setup(l => l[SharedResourcesKeys.AddFailed])
                .Returns(new LocalizedString(SharedResourcesKeys.AddFailed, "Add failed"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Add failed");
        }

        #endregion

        #region EditShipping_MethodCommand Tests

        [Fact]
        public async Task Handle_EditShipping_MethodCommand_ShouldReturnSuccess_WhenUpdated()
        {
            var command = new EditShipping_MethodCommand { Id = 1, Method_Name = "Updated" };
            var existingEntity = new Shipping_Methods { Id = 1, Method_Name = "Old" };
            var updatedEntity = new Shipping_Methods { Id = 1, Method_Name = "Updated" };
            var dto = new Shipping_MethodCommand { Id = 1, Method_Name = "Updated" };

            _serviceMock.Setup(s => s.GetShipping_MethodByIdAsync(1)).ReturnsAsync(existingEntity);
            _serviceMock.Setup(s => s.EditAsync(It.IsAny<Shipping_Methods>())).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task Handle_EditShipping_MethodCommand_ShouldReturnNotFound_WhenEntityNotExist()
        {
            var command = new EditShipping_MethodCommand { Id = 99, Method_Name = "New" };

            _serviceMock.Setup(s => s.GetShipping_MethodByIdAsync(99)).ReturnsAsync((Shipping_Methods?)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditShipping_MethodCommand_ShouldReturnBadRequest_WhenUpdateFailed()
        {
            var command = new EditShipping_MethodCommand { Id = 1, Method_Name = "New" };
            var existingEntity = new Shipping_Methods { Id = 1, Method_Name = "Old" };
            var updatedEntity = new Shipping_Methods { Id = 1, Method_Name = "New" };

            _serviceMock.Setup(s => s.GetShipping_MethodByIdAsync(1)).ReturnsAsync(existingEntity);
            _serviceMock.Setup(s => s.EditAsync(updatedEntity)).ReturnsAsync("Fail");

            _localizerMock.Setup(l => l[SharedResourcesKeys.UpdateFailed])
                .Returns(new LocalizedString(SharedResourcesKeys.UpdateFailed, "Update failed"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Update failed");
        }

        #endregion

        #region DeleteShipping_MethodCommand Tests

        [Fact]
        public async Task Handle_DeleteShipping_MethodCommand_ShouldReturnDeleted_WhenSuccess()
        {
            var command = new DeleteShipping_MethodCommand(1);
            var existingEntity = new Shipping_Methods { Id = 1, Method_Name = "Express" };

            _serviceMock.Setup(s => s.GetShipping_MethodByIdAsync(1)).ReturnsAsync(existingEntity);
            _serviceMock.Setup(s => s.DeleteAsync(existingEntity)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_DeleteShipping_MethodCommand_ShouldReturnNotFound_WhenEntityNotExist()
        {
            var command = new DeleteShipping_MethodCommand(99);

            _serviceMock.Setup(s => s.GetShipping_MethodByIdAsync(99)).ReturnsAsync((Shipping_Methods?)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteShipping_MethodCommand_ShouldReturnBadRequest_WhenDeleteFailed()
        {
            var command = new DeleteShipping_MethodCommand(1);
            var existingEntity = new Shipping_Methods { Id = 1, Method_Name = "Express" };

            _serviceMock.Setup(s => s.GetShipping_MethodByIdAsync(1)).ReturnsAsync(existingEntity);
            _serviceMock.Setup(s => s.DeleteAsync(existingEntity)).ReturnsAsync("Fail");
            _localizerMock.Setup(l => l[SharedResourcesKeys.DeletedFailed])
                .Returns(new LocalizedString(SharedResourcesKeys.DeletedFailed, "Delete failed"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Delete failed");
        }

        #endregion

        #endregion

    }
}
