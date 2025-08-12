using BookShop.Core.Features.CartItem.Commands.Handlers;
using BookShop.Core.Features.CartItem.Commands.Models;
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
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.CartItemsTest.Commands
{
    public class CartItemCommandHandlerTests
    {
        #region Fields
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ICartItemService> _cartItemServiceMock;
        private readonly Mock<IShoppingCartService> _shoppingCartServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly CartItemCommandHandler _handler;

        #endregion

        #region Constructor
        public CartItemCommandHandlerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _cartItemServiceMock = new Mock<ICartItemService>();
            _shoppingCartServiceMock = new Mock<IShoppingCartService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _handler = new CartItemCommandHandler(
                _cartItemServiceMock.Object,
                _bookServiceMock.Object,
                _shoppingCartServiceMock.Object,
                _currentUserServiceMock.Object,
                _dbContextMock.Object,
                _localizerMock.Object
            );
        }
        #endregion

        #region Tests

        #region AddCartItemCommand Tests

        [Fact]
        public async Task AddCartItem_Should_Return_Unprocessable_If_Book_Not_In_Stock()
        {
            // Arrange
            var cmd = new AddCartItemCommand { BookId = 1, Quantity = 1 };
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(1);
            _bookServiceMock.Setup(s => s.IsTheBookInStock(cmd.BookId)).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task AddCartItem_Should_Return_Unprocessable_If_Book_Already_In_Cart()
        {
            var cmd = new AddCartItemCommand { BookId = 1, Quantity = 1 };
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(1);

            var cart = new ShoppingCart { Id = 10, ApplicationUserId = 1 };
            _shoppingCartServiceMock.Setup(s => s.GetByUserId(1)).ReturnsAsync(cart);
            _bookServiceMock.Setup(s => s.IsTheBookInStock(1)).ReturnsAsync(true);
            _cartItemServiceMock.Setup(s => s.IsCartItemExistByBookIdAndShoppingCartId(1, cart.Id))
                                .ReturnsAsync(true);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task AddCartItem_Should_Return_NotFound_If_Book_Not_Exist()
        {
            var cmd = new AddCartItemCommand { BookId = 1, Quantity = 1 };
            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(1);

            _shoppingCartServiceMock.Setup(s => s.GetByUserId(1)).ReturnsAsync((ShoppingCart)null!);
            _bookServiceMock.Setup(s => s.IsTheBookInStock(1)).ReturnsAsync(true);
            _bookServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Book)null!);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddCartItem_Should_Return_Created_When_Success()
        {
            // Arrange
            var createdCart = new ShoppingCart
            {
                Id = 123,
                ApplicationUserId = 1,
                CreatedAt = DateTime.UtcNow
            };

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

            var cmd = new AddCartItemCommand { BookId = 1, Quantity = 1 };

            _currentUserServiceMock.Setup(s => s.GetUserId()).Returns(1);
            _shoppingCartServiceMock
                        .SetupSequence(s => s.GetByUserId(1))
                        .ReturnsAsync((ShoppingCart)null!) // أول مرة: مفيش كارت
                        .ReturnsAsync(createdCart);        // تاني مرة: الكارت اللي لسه اتعمل

            _bookServiceMock.Setup(s => s.IsTheBookInStock(1)).ReturnsAsync(true);

            _bookServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new Book { Id = 1 });

            _shoppingCartServiceMock
                .Setup(s => s.AddAsync(It.IsAny<ShoppingCart>()))
                .ReturnsAsync("Success");

            _cartItemServiceMock
                .Setup(s => s.AddAsync(It.IsAny<BookShop.DataAccess.Entities.CartItem>()))
                .ReturnsAsync("Success");

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        #endregion

        #endregion

    }
}
