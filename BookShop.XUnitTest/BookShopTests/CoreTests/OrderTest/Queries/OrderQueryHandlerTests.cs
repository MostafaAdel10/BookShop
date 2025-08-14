using AutoMapper;
using BookShop.Core.Features.Order.Queries.Handlers;
using BookShop.Core.Features.Order.Queries.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Entities.Identity;
using BookShop.DataAccess.Enums;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.OrderTest.Queries
{
    public class OrderQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly OrderQueryHandler _handler;

        #endregion

        #region Constructor
        public OrderQueryHandlerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _mapperMock = new Mock<IMapper>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _handler = new OrderQueryHandler(
                _orderServiceMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object
            );
        }
        #endregion

        #region handle Functions Test

        #region GetOrdersByCurrentUserId Tests

        [Fact]
        public async Task GetOrdersByCurrentUserId_Should_Return_NotFound_When_NoOrders()
        {
            // Arrange
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(1);
            _orderServiceMock.Setup(s => s.GetOrdersByUserIdAsync(1))
                             .ReturnsAsync(new List<BookShop.DataAccess.Entities.Order>());

            var query = new GetOrdersByCurrentUserIdQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetOrdersByCurrentUserId_Should_Return_Success_When_OrdersExist()
        {
            // Arrange
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(1);
            _orderServiceMock.Setup(s => s.GetOrdersByUserIdAsync(1))
                .ReturnsAsync(new List<Order>
                {
                    new Order
                    {
                        Id = 1,
                        OrderDate = DateTime.UtcNow,
                        EstimatedDeliveryTime = DateTime.UtcNow.AddDays(3),
                        Total_amout = 100,
                        tracking_number = "TRK123",
                        ShippingMethodsID = 1,
                        OrderStateID = 1,
                        ApplicationUserId = 1,
                        ApplicationUser = new ApplicationUser
                        {
                            Id = 1,
                            UserName = "TestUser"
                        },
                        Payment = new Payment
                        {
                            PaymentMethod = PaymentMethodType.VodafoneCash // أو أي قيمة مناسبة
                        },
                        shipping_Methods = new Shipping_Methods
                        {
                            Method_Name = "Express",
                            Cost = 10
                        },
                        order_State = new Order_State
                        {
                            Name = "Pending",
                            Name_Ar = "قيد الانتظار"
                        },
                        Address = new Address
                        {
                            FullName = "Test User",
                            AddressLine1 = "123 Main St",
                            City = "Test City",
                            State = "Test State",
                            PostalCode = "12345",
                            Country = "Test Country",
                            PhoneNumber = "1234567890"
                        },
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                Id = 1,
                                Quantity = 2,
                                BookId = 1,
                                Price = 50,
                                OrderId = 1,
                                book = new Book
                                {
                                    Id = 1,
                                    Title = "Sample Book",
                                    Price = 50
                                }
                            }
                        }
                    }
                });

            var query = new GetOrdersByCurrentUserIdQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
        }

        #endregion

        #region GetOrderByIdQuery Tests

        [Fact]
        public async Task GetOrderById_Should_Return_NotFound_When_OrderDoesNotExist()
        {
            // Arrange
            _orderServiceMock.Setup(s => s.GetByIdAsyncWithInclude(It.IsAny<int>()))
                             .ReturnsAsync((BookShop.DataAccess.Entities.Order)null!);

            var query = new GetOrderByIdQuery(999);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetOrderById_Should_Return_Success_When_OrderExists()
        {
            // Arrange
            _orderServiceMock.Setup(s => s.GetByIdAsyncWithInclude(It.IsAny<int>()))
                 .ReturnsAsync(new BookShop.DataAccess.Entities.Order
                 {
                     Id = 1,
                     OrderDate = DateTime.UtcNow,
                     EstimatedDeliveryTime = DateTime.UtcNow.AddDays(3),
                     Total_amout = 100,
                     tracking_number = "TRK123",
                     ShippingMethodsID = 1,
                     OrderStateID = 1,
                     ApplicationUserId = 1,
                     ApplicationUser = new ApplicationUser
                     {
                         Id = 1,
                         UserName = "TestUser"
                     },
                     Payment = new Payment
                     {
                         PaymentMethod = PaymentMethodType.VodafoneCash // أو أي قيمة مناسبة
                     },
                     shipping_Methods = new Shipping_Methods
                     {
                         Method_Name = "Express",
                         Cost = 10
                     },
                     order_State = new Order_State
                     {
                         Name = "Pending",
                         Name_Ar = "قيد الانتظار"
                     },
                     Address = new Address
                     {
                         FullName = "Test User",
                         AddressLine1 = "123 Main St",
                         City = "Test City",
                         State = "Test State",
                         PostalCode = "12345",
                         Country = "Test Country",
                         PhoneNumber = "1234567890"
                     },
                     OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    Quantity = 2,
                    BookId = 1,
                    Price = 50,
                    OrderId = 1,
                    book = new Book
                    {
                        Id = 1,
                        Title = "Sample Book",
                        Price = 50
                    }
                }
            }
                 });

            var query = new GetOrderByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Data.Id.Should().Be(1);
        }

        #endregion

        #region GetOrderList Tests

        [Fact]
        public async Task GetOrderList_ShouldReturnMappedOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                OrderDate = DateTime.UtcNow,
                Total_amout = 100,
                tracking_number = "TR123",
                shipping_Methods = new Shipping_Methods { Method_Name = "Fast", Cost = 10 },
                EstimatedDeliveryTime = DateTime.UtcNow.AddDays(2),
                ApplicationUserId = 99,
                ApplicationUser = new ApplicationUser { UserName = "John" },
                order_State = new Order_State { Name = "Pending", Name_Ar = "قيد الانتظار" },
                Payment = new Payment { PaymentMethod = PaymentMethodType.VodafoneCash},
                Address = new Address
                {
                    FullName = "John Doe",
                    AddressLine1 = "123 St",
                    City = "CityX",
                    State = "StateX",
                    PostalCode = "12345",
                    Country = "CountryX",
                    PhoneNumber = "123456789"
                },
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 5,
                        BookId = 10,
                        Quantity = 2,
                        Price = 50,
                        OrderId = 1,
                        book = new Book { Title = "Book 1" }
                    }
                }
            }
        };

            var mockDbSet = orders.AsQueryable().BuildMockDbSet();
            _orderServiceMock.Setup(s => s.GetOrderQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _handler.Handle(new GetOrderListQuery(), CancellationToken.None);

            // Assert
            result.Data.Should().HaveCount(1);
            var order = result.Data.First();
            order.ShippingMethod.Should().Be("Fast");
            order.UserName.Should().Be("John");
            order.OrderItems.Should().ContainSingle();
            result.Meta.Should().BeEquivalentTo(new { Count = 1 });
        }

        [Fact]
        public async Task GetOrderList_ShouldReturnEmptyList_WhenNoOrdersExist()
        {
            // Arrange
            var emptyOrders = new List<Order>();
            var mockDbSet = emptyOrders.AsQueryable().BuildMockDbSet();
            _orderServiceMock.Setup(s => s.GetOrderQueryable()).Returns(mockDbSet.Object);

            // Act
            var result = await _handler.Handle(new GetOrderListQuery(), CancellationToken.None);

            // Assert
            result.Data.Should().BeEmpty();
            result.Meta.Should().BeEquivalentTo(new { Count = 0 });
        }

        #endregion

        #region GetOrderPaginatedList Tests

        [Fact]
        public async Task GetOrderPaginatedList_ShouldReturnPagedResult()
        {
            // Arrange
            var request = new GetOrderPaginatedListQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Search = "",
                OrderBy = OrderOrderingEnum.Code
            };

            var orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                OrderDate = DateTime.UtcNow,
                Total_amout = 100,
                tracking_number = "TR123",
                shipping_Methods = new Shipping_Methods { Method_Name = "Fast", Cost = 10 },
                EstimatedDeliveryTime = DateTime.UtcNow.AddDays(2),
                ApplicationUserId = 99,
                ApplicationUser = new ApplicationUser { UserName = "John" },
                order_State = new Order_State { Name = "Pending", Name_Ar = "قيد الانتظار" },
                Payment = new Payment { PaymentMethod = PaymentMethodType.VodafoneCash },
                Address = new Address
                {
                    FullName = "John Doe",
                    AddressLine1 = "123 St",
                    City = "CityX",
                    State = "StateX",
                    PostalCode = "12345",
                    Country = "CountryX",
                    PhoneNumber = "123456789"
                },
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 5,
                        BookId = 10,
                        Quantity = 2,
                        Price = 50,
                        OrderId = 1,
                        book = new Book { Title = "Book 1" }
                    }
                }
            }
        };

            var mockDbSet = orders.AsQueryable().BuildMockDbSet();
            _orderServiceMock
                .Setup(s => s.FilterOrderPaginatedQueryable(request.OrderBy, request.Search))
                .Returns(mockDbSet.Object);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Data.Should().HaveCount(1);
            result.Meta.Should().BeEquivalentTo(new { Count = 1 });
        }

        #endregion

        #endregion

    }
}
