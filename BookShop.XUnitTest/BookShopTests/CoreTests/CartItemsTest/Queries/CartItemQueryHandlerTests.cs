using AutoMapper;
using BookShop.Core.Features.CartItem.Queries.Handlers;
using BookShop.Core.Features.CartItem.Queries.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.CartItemsTest.Queries
{
    public class CartItemQueryHandlerTests
    {
        #region Fields
        private readonly Mock<ICartItemService> _cartItemServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly CartItemQueryHandler _handler;
        #endregion

        #region Constructor
        public CartItemQueryHandlerTests()
        {
            _cartItemServiceMock = new Mock<ICartItemService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _mapperMock = new Mock<IMapper>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _handler = new CartItemQueryHandler(
                _cartItemServiceMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object
            );
        }
        #endregion

        #region handle functions tests

        #region GetCartItemsByCurrentUserIdQuery

        [Fact]
        public async Task Handle_Should_Return_Success_When_CartItemsExist()
        {
            // Arrange
            var userId = 123;
            var cartItems = new List<BookShop.DataAccess.Entities.CartItem>
            {
                new BookShop.DataAccess.Entities.CartItem
                {
                    Id = 1,
                    BookId = 101,
                    Quantity = 2,
                    ShoppingCartId = 5,
                    Book = new Book { Id = 101, Title = "Test Book", Price = 100, PriceAfterDiscount = 80 }
                }
            };

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _cartItemServiceMock.Setup(x => x.GetCartItemsByUserIdAsync(userId)).ReturnsAsync(cartItems);

            // Act
            var result = await _handler.Handle(new GetCartItemsByCurrentUserIdQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().HaveCount(1);
            result.Meta.Should().BeEquivalentTo(new { Count = 1 });
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_CartItemsListIsEmpty()
        {
            // Arrange
            var userId = 123;

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);

            _cartItemServiceMock.Setup(x => x.GetCartItemsByUserIdAsync(userId))
                                .ReturnsAsync(new List<BookShop.DataAccess.Entities.CartItem>());

            // Act
            var result = await _handler.Handle(new GetCartItemsByCurrentUserIdQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_CartItemsIsNull()
        {
            // Arrange
            var userId = 123;

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);

            _cartItemServiceMock.Setup(x => x.GetCartItemsByUserIdAsync(userId))
                                .ReturnsAsync((List<BookShop.DataAccess.Entities.CartItem>)null!);

            // Act
            var result = await _handler.Handle(new GetCartItemsByCurrentUserIdQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }

        #endregion

        #endregion

    }
}
