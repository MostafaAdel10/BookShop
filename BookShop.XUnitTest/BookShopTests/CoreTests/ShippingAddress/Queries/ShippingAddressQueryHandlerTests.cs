using AutoMapper;
using BookShop.Core.Features.ShippingAddress.Queries.Handlers;
using BookShop.Core.Features.ShippingAddress.Queries.Models;
using BookShop.Core.Mapping.ShippingAddresse;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.ShippingAddress.Queries
{
    public class ShippingAddressQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IAddressService> _addressServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly IMapper _mapper;
        private readonly ShippingAddressProfile _shippingAddressProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly ShippingAddressQueryHandler _handler;
        #endregion

        #region Constructors
        public ShippingAddressQueryHandlerTests()
        {
            _addressServiceMock = new Mock<IAddressService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _shippingAddressProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_shippingAddressProfile));
            _mapper = new Mapper(configuration);

            _handler = new ShippingAddressQueryHandler(
                _addressServiceMock.Object,
                _mapper,
                _currentUserServiceMock.Object,
                _localizerMock.Object
            );
        }
        #endregion

        #region handle functions tests

        #region GetShippingAddressesByCurrentUserIdQuery

        [Fact]
        public async Task Handle_Should_Return_Success_When_AddressesExist()
        {
            // Arrange
            var userId = 123;
            var addresses = new List<Address>
            {
                new Address { Id = 1, FullName = "Test User", AddressLine1 = "123 Street" }
            };

            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _addressServiceMock.Setup(x => x.GetAddressesByUserIdAsync(userId))
                               .ReturnsAsync(addresses);

            // Act
            var result = await _handler.Handle(new GetShippingAddressesByCurrentUserIdQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_AddressesListIsEmpty()
        {
            // Arrange
            var userId = 123;
            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _addressServiceMock.Setup(x => x.GetAddressesByUserIdAsync(userId))
                               .ReturnsAsync(new List<Address>());

            // Act
            var result = await _handler.Handle(new GetShippingAddressesByCurrentUserIdQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_AddressesIsNull()
        {
            // Arrange
            var userId = 123;
            _currentUserServiceMock.Setup(x => x.GetUserId()).Returns(userId);
            _addressServiceMock.Setup(x => x.GetAddressesByUserIdAsync(userId))
                               .ReturnsAsync((List<Address>)null!);

            // Act
            var result = await _handler.Handle(new GetShippingAddressesByCurrentUserIdQuery(), CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Data.Should().BeNull();
        }

        #endregion

        #endregion
    }
}
