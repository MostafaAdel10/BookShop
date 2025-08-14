using BookShop.Core.Features.Order.Commands.Handlers;
using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Data;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.OrderTest.Commands
{
    public class OrderCommandHandlerTests
    {
        #region Fields
        private readonly Mock<IOrderService> _orderServiceMock = new();
        private readonly Mock<IOrderItemService> _orderItemServiceMock = new();
        private readonly Mock<IShipping_MethodService> _shippingMethodServiceMock = new();
        private readonly Mock<IBookService> _bookServiceMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<ICartItemService> _cartItemServiceMock = new();
        private readonly Mock<IPaymentService> _paymentServiceMock = new();
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock = new();
        private readonly Mock<ApplicationDbContext> _dbContextMock = new();

        private readonly OrderCommandHandler _handler;
        #endregion

        #region Constructor
        public OrderCommandHandlerTests()
        {
            _handler = new OrderCommandHandler(
                _orderServiceMock.Object,
                _localizerMock.Object,
                _shippingMethodServiceMock.Object,
                _bookServiceMock.Object,
                _orderItemServiceMock.Object,
                _currentUserServiceMock.Object,
                _cartItemServiceMock.Object,
                _paymentServiceMock.Object,
                _dbContextMock.Object
            );
        }
        #endregion

        #region AddOrderCommandHandler Tests

        [Fact]
        public async Task Handle_ShouldReturnUnprocessableEntity_WhenCartIsEmpty()
        {
            // Arrange
            var command = new AddOrderCommand { ShippingMethodId = 1, PaymentMethodType = PaymentMethodType.VodafoneCash };
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);
            _cartItemServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<CartItem>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenShippingMethodNotFound()
        {
            var command = new AddOrderCommand { ShippingMethodId = 99 };
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);
            _cartItemServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<CartItem> { new CartItem { Book = new Book { PriceAfterDiscount = 10 }, Quantity = 1 } });
            _shippingMethodServiceMock.Setup(s => s.GetShipping_MethodByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Shipping_Methods)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Handle_ShouldReturnUnprocessableEntity_WhenBookQuantityIsInsufficient()
        {
            var command = new AddOrderCommand
            {
                ShippingMethodId = 1,
                PaymentMethodType = PaymentMethodType.VodafoneCash,
                FullName = "Test User",
                AddressLine1 = "123 Test St",
                City = "Test City",
                State = "Test State",
                PostalCode = "12345",
                Country = "Test Country",
                PhoneNumber = "1234567890"
            };

            // Mocking the transaction
            var mockTransaction = new Mock<IDbContextTransaction>();
            mockTransaction
                .Setup(t => t.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockTransaction
                .Setup(t => t.RollbackAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var dbFacadeMock = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            dbFacadeMock
                .Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            _dbContextMock.Setup(x => x.Database).Returns(dbFacadeMock.Object);

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);
            _cartItemServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<CartItem>
                {
                new CartItem { BookId = 1, Book = new Book { PriceAfterDiscount = 10 }, Quantity = 5 }
                });
            _shippingMethodServiceMock.Setup(s => s.GetShipping_MethodByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Shipping_Methods { Cost = 5, DeliveryDurationInDays = 3 });
            _bookServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Book { Unit_Instock = 2 });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenOrderIsCreatedSuccessfully()
        {
            var command = new AddOrderCommand { ShippingMethodId = 1, PaymentMethodType = PaymentMethodType.EtisalatCash, FullName = "Test" };

            // Mocking the transaction
            var mockTransaction = new Mock<IDbContextTransaction>();
            mockTransaction
                .Setup(t => t.CommitAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockTransaction
                .Setup(t => t.RollbackAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var dbFacadeMock = new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            dbFacadeMock
                .Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            _dbContextMock.Setup(x => x.Database).Returns(dbFacadeMock.Object);

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(123);

            var cartItems = new List<CartItem>
        {
            new CartItem { BookId = 1, Book = new Book { PriceAfterDiscount = 10 }, Quantity = 2 }
        };

            _cartItemServiceMock.Setup(s => s.GetCartItemsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(cartItems);
            _shippingMethodServiceMock.Setup(s => s.GetShipping_MethodByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Shipping_Methods { Cost = 5, DeliveryDurationInDays = 3 });
            _bookServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Book { Unit_Instock = 10 });
            _orderServiceMock.Setup(s => s.AddAsyncReturnId(It.IsAny<Order>()))
                .ReturnsAsync(new Order { Id = 123 });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(123);

            _orderItemServiceMock.Verify(s => s.AddRangeAsync(It.IsAny<List<OrderItem>>()), Times.Once);
            _cartItemServiceMock.Verify(s => s.DeleteCartItemsByUserIdAsync(123), Times.Once);
            _paymentServiceMock.Verify(s => s.AddAsync(It.IsAny<Payment>()), Times.Once);
            _paymentServiceMock.Verify(s => s.EditAsync(It.IsAny<Payment>()), Times.Once);
        }

        #endregion

    }
}
