using AutoMapper;
using BookShop.Core.Features.User.Queries.Handlers;
using BookShop.Core.Features.User.Queries.Models;
using BookShop.Core.Mapping.User;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.UserTest.Queries
{
    public class UserQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly UserProfile _userProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly UserQueryHandler _handler;

        public UserQueryHandlerTests()
        {
            _userProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_userProfile));
            _mapper = new Mapper(configuration);

            _localizerMock = new();
            _localizerMock.Setup(l => l[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _handler = new UserQueryHandler(
                _localizerMock.Object,
                _mapper,
                _userManagerMock.Object
            );
        }

        [Fact]
        public async Task Handle_GetUserById_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            var userId = 123;
            var userId_s = "123";
            var user = new ApplicationUser { Id = userId, UserName = "TestUser" };

            _userManagerMock.Setup(m => m.FindByIdAsync(userId_s)).ReturnsAsync(user);

            var query = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 999;
            var userId_s = "999";
            _userManagerMock.Setup(m => m.FindByEmailAsync(userId_s)).ReturnsAsync((ApplicationUser)null!);

            var query = new GetUserByIdQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result.Message.Should().Be(SharedResourcesKeys.NotFound);
        }

    }
}
