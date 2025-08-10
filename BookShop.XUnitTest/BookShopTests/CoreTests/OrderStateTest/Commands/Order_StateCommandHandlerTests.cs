using AutoMapper;
using BookShop.Core.Features.Order_State.Commands.Handlers;
using BookShop.Core.Features.Order_State.Commands.Models;
using BookShop.Core.Mapping.Order_State;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.OrderStateTest.Commands
{
    public class Order_StateCommandHandlerTests
    {
        #region Fields
        private readonly Mock<IOrder_StateService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly Order_StateProfile _order_StateProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Order_StateCommandHandler _handler;
        #endregion

        #region Constructors
        public Order_StateCommandHandlerTests()
        {
            _serviceMock = new Mock<IOrder_StateService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _order_StateProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_order_StateProfile));
            _mapper = new Mapper(configuration);

            _handler = new Order_StateCommandHandler(
                _serviceMock.Object,
                _mapper,
                _localizerMock.Object
            );
        }
        #endregion

        #region Handel Functions Test

        #region AddOrder_StateCommand Tests

        [Fact]
        public async Task Handle_AddOrder_StateCommand_ShouldReturnCreated_WhenSuccess()
        {
            // Arrange
            var command = new AddOrder_StateCommand { Name = "Test", Name_Ar = "اختبار" };
            var dto = new Order_StateCommand { Name = "Test", Name_Ar = "اختبار" };

            _serviceMock.Setup(s => s.AddAsync(It.IsAny<Order_State>())).ReturnsAsync("Success");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            result.Data.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task Handle_AddOrder_StateCommand_ShouldReturnBadRequest_WhenFail()
        {
            // Arrange
            var command = new AddOrder_StateCommand();

            _serviceMock.Setup(s => s.AddAsync(It.IsAny<Order_State>())).ReturnsAsync("Error");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        #endregion

        #region EditOrder_StateCommand Tests

        [Fact]
        public async Task Handle_EditOrder_StateCommand_ShouldReturnSuccess_WhenFoundAndUpdated()
        {
            // Arrange
            var command = new EditOrder_StateCommand { Id = 1, Name = "Updated" };
            var entity = new Order_State { Id = 1, Name = "Old" };

            _serviceMock.Setup(s => s.GetOrder_StateById(command.Id)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.EditAsync(It.IsAny<Order_State>())).ReturnsAsync("Success");

            _localizerMock.Setup(l => l[SharedResourcesKeys.Updated])
                          .Returns(new LocalizedString(SharedResourcesKeys.Updated, "Updated successfully"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_EditOrder_StateCommand_ShouldReturnNotFound_WhenNotFound()
        {
            // Arrange
            var command = new EditOrder_StateCommand { Id = 1 };
            _serviceMock.Setup(s => s.GetOrder_StateById(command.Id)).ReturnsAsync((Order_State)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditOrder_StateCommand_ShouldReturnBadRequest_WhenUpdateFails()
        {
            // Arrange
            var command = new EditOrder_StateCommand { Id = 1, Name = "Fail" };
            var entity = new Order_State { Id = 1, Name = "Old" };

            _serviceMock.Setup(s => s.GetOrder_StateById(command.Id)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.EditAsync(entity)).ReturnsAsync("Error");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        #endregion

        #region DeleteOrder_StateCommand Tests

        [Fact]
        public async Task Handle_DeleteOrder_StateCommand_ShouldReturnDeleted_WhenSuccess()
        {
            // Arrange
            var command = new DeleteOrder_StateCommand(1);
            var entity = new Order_State { Id = 1 };

            _serviceMock.Setup(s => s.GetOrder_StateById(command.Id)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.DeleteAsync(entity)).ReturnsAsync("Success");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_DeleteOrder_StateCommand_ShouldReturnNotFound_WhenNotFound()
        {
            // Arrange
            var command = new DeleteOrder_StateCommand(1);
            _serviceMock.Setup(s => s.GetOrder_StateById(command.Id)).ReturnsAsync((Order_State)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteOrder_StateCommand_ShouldReturnBadRequest_WhenFail()
        {
            // Arrange
            var command = new DeleteOrder_StateCommand(1);
            var entity = new Order_State { Id = 1 };

            _serviceMock.Setup(s => s.GetOrder_StateById(command.Id)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.DeleteAsync(entity)).ReturnsAsync("Error");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        #endregion

        #endregion

    }
}
