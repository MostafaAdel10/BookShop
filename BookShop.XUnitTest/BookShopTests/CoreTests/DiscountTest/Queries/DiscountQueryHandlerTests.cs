using AutoMapper;
using BookShop.Core.Features.Discount.Queries.Handlers;
using BookShop.Core.Features.Discount.Queries.Models;
using BookShop.Core.Mapping.Discounts;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.DiscountTest.Queries
{
    public class DiscountQueryHandlerTests
    {
        #region Fields
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly IMapper _mapper;
        private readonly DiscountProfile _discountProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly DiscountQueryHandler _handler;
        #endregion

        #region Constructors
        public DiscountQueryHandlerTests()
        {
            _discountServiceMock = new();
            _localizerMock = new();
            _discountProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_discountProfile));
            _mapper = new Mapper(configuration);

            _handler = new DiscountQueryHandler(_discountServiceMock.Object, _mapper, _localizerMock.Object);
        }
        #endregion

        #region Handel Functions Test

        #region GetDiscountListQuery Tests

        [Fact]
        public async Task Handle_GetDiscountListQuery_ShouldReturnMappedList_WithMetaCount()
        {
            // Arrange
            var discounts = new List<Discount>
            {
                new Discount { Id = 1, Name = "Summer", Percentage = 10 },
                new Discount { Id = 2, Name = "Winter", Percentage = 20 }
            };

            _discountServiceMock.Setup(x => x.GetDiscountsListAsync())
                .ReturnsAsync(discounts);

            var request = new GetDiscountListQuery();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Meta.Should().NotBeNull();
            result.Meta.Should().BeEquivalentTo(new { Count = 2 });
        }

        #endregion

        #region GetDiscountByIdQuery Tests

        [Fact]
        public async Task Handle_GetDiscountByIdQuery_ShouldReturnMappedDiscount_WhenExists()
        {
            // Arrange
            var discount = new Discount { Id = 1, Name = "New Year", Percentage = 15 };

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(1))
                .ReturnsAsync(discount);

            var request = new GetDiscountByIdQuery(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GetDiscountByIdQuery_ShouldReturnNotFound_WhenDiscountDoesNotExist()
        {
            // Arrange
            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(99))
                .ReturnsAsync((Discount)null!);

            var localized = new LocalizedString(nameof(SharedResourcesKeys.NotFound), "Discount not found");
            _localizerMock.Setup(x => x[SharedResourcesKeys.NotFound]).Returns(localized);

            var request = new GetDiscountByIdQuery(99);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Message.Should().Be("Discount not found");
        }

        #endregion

        #endregion

    }
}
