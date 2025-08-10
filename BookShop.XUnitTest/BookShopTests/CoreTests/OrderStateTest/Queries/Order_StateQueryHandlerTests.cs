using AutoMapper;
using BookShop.Core.Features.Order_State.Queries.Handlers;
using BookShop.Core.Features.Order_State.Queries.Models;
using BookShop.Core.Mapping.Order_State;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.OrderStateTest.Queries
{
    public class Order_StateQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IOrder_StateService> _orderStateServiceMock;
        private readonly IMapper _mapper;
        private readonly Order_StateProfile _order_StateProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Order_StateQueryHandler _handler;
        #endregion

        #region Constructors
        public Order_StateQueryHandlerTests()
        {
            _orderStateServiceMock = new Mock<IOrder_StateService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _order_StateProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_order_StateProfile));
            _mapper = new Mapper(configuration);

            _handler = new Order_StateQueryHandler(
                _orderStateServiceMock.Object,
                _mapper,
                _localizerMock.Object
            );
        }
        #endregion

        #region Handel Functions Test

        #region GetOrder_StateListQuery Tests

        [Fact]
        public async Task Handle_GetOrder_StateListQuery_ShouldReturnListWithMeta()
        {
            // Arrange
            var query = new GetOrder_StateListQuery();
            var entities = new List<Order_State>
            {
                new Order_State { Id = 1, Name = "Pending", Name_Ar = "قيد الانتظار" },
                new Order_State { Id = 2, Name = "Shipped", Name_Ar = "تم الشحن" }
            };

            _orderStateServiceMock.Setup(s => s.GetOrder_StatesListAsync())
                .ReturnsAsync(entities);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Meta.Should().BeEquivalentTo(new { Count = 2 });
        }

        #endregion

        #region GetOrder_StateByIdQuery Tests

        [Fact]
        public async Task Handle_GetOrder_StateByIdQuery_WhenFound_ShouldReturnSuccess()
        {
            // Arrange
            var query = new GetOrder_StateByIdQuery(1);
            var entity = new Order_State { Id = 1, Name = "Pending", Name_Ar = "قيد الانتظار" };

            _orderStateServiceMock.Setup(s => s.GetOrder_StateById(query.Id))
                .ReturnsAsync(entity);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GetOrder_StateByIdQuery_WhenNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var query = new GetOrder_StateByIdQuery(99);

            _orderStateServiceMock.Setup(s => s.GetOrder_StateById(query.Id))
                .ReturnsAsync((Order_State)null!);

            var localizedString = new LocalizedString(SharedResourcesKeys.NotFound, "Not Found");
            _localizerMock.Setup(l => l[SharedResourcesKeys.NotFound])
                .Returns(localizedString);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Not Found");
        }

        #endregion

        #endregion

    }
}
