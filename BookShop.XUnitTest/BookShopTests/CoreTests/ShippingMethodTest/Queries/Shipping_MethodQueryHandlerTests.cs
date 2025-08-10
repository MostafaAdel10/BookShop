using AutoMapper;
using BookShop.Core.Features.Shipping_Method.Queries.Handlers;
using BookShop.Core.Features.Shipping_Method.Queries.Models;
using BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_;
using BookShop.Core.Mapping.Shipping_Method;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.ShippingMethodTest.Queries
{
    public class Shipping_MethodQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IShipping_MethodService> _shippingMethodServiceMock;
        private readonly IMapper _mapper;
        private readonly Shipping_MethodProfile _shipping_MethodProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Shipping_MethodQueryHandler _handler;
        #endregion

        #region Constructors
        public Shipping_MethodQueryHandlerTests()
        {
            _shippingMethodServiceMock = new Mock<IShipping_MethodService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();

            _shipping_MethodProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_shipping_MethodProfile));
            _mapper = new Mapper(configuration);

            _handler = new Shipping_MethodQueryHandler(
                _shippingMethodServiceMock.Object,
                _mapper,
                _localizerMock.Object
            );
        }
        #endregion

        #region Handel Functions Test

        #region GetShipping_MethodListQuery Tests

        [Fact]
        public async Task Handle_GetShipping_MethodListQuery_ShouldReturnSuccess_WithData()
        {
            // Arrange
            var entities = new List<Shipping_Methods>
            {
                new Shipping_Methods { Id = 1, Method_Name = "Fast" ,DeliveryDurationInDays = 2},
                new Shipping_Methods { Id = 2, Method_Name = "Slow" ,DeliveryDurationInDays= 5}
            };

            var mappedDtos = new List<GetShipping_MethodListResponse>
            {
                new GetShipping_MethodListResponse { Id = 1, Method_Name = "Fast" ,DeliveryDurationInDays = 2},
                new GetShipping_MethodListResponse { Id = 2, Method_Name = "Slow" ,DeliveryDurationInDays = 5}
            };

            _shippingMethodServiceMock
                .Setup(s => s.GetShipping_MethodsListAsync())
                .ReturnsAsync(entities);

            var query = new GetShipping_MethodListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(mappedDtos);
            result.Meta.Should().BeEquivalentTo(new { Count = mappedDtos.Count });
        }

        [Fact]
        public async Task Handle_GetShipping_MethodListQuery_ShouldReturnSuccess_WithEmptyList()
        {
            // Arrange
            var entities = new List<Shipping_Methods>();
            var mappedDtos = new List<GetShipping_MethodListResponse>();

            _shippingMethodServiceMock
                .Setup(s => s.GetShipping_MethodsListAsync())
                .ReturnsAsync(entities);

            var query = new GetShipping_MethodListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().BeEmpty();
            result.Meta.Should().BeEquivalentTo(new { Count = 0 });
        }

        #endregion

        #region GetShipping_MethodByIdQuery Tests

        [Fact]
        public async Task Handle_GetShipping_MethodByIdQuery_ShouldReturnSuccess_WhenFound()
        {
            // Arrange
            var entity = new Shipping_Methods { Id = 1, Method_Name = "Fast", DeliveryDurationInDays = 2 };

            _shippingMethodServiceMock
                .Setup(s => s.GetShipping_MethodByIdAsync(1))
                .ReturnsAsync(entity);

            var query = new GetShipping_MethodByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GetShipping_MethodByIdQuery_ShouldReturnNotFound_WhenNotExist()
        {
            // Arrange
            _shippingMethodServiceMock
                .Setup(s => s.GetShipping_MethodByIdAsync(1))
                .ReturnsAsync((Shipping_Methods?)null!);

            _localizerMock
                .Setup(l => l[SharedResourcesKeys.NotFound])
                .Returns(new LocalizedString(SharedResourcesKeys.NotFound, "Not Found"));

            var query = new GetShipping_MethodByIdQuery(1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Not Found");
        }

        #endregion

        #endregion

    }
}
