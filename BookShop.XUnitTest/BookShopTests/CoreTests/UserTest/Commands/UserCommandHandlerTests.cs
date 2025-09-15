using AutoMapper;
using BookShop.Core.Features.User.Commands.Handlers;
using BookShop.Core.Features.User.Commands.Models;
using BookShop.Core.Mapping.User;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities.Identity;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using MockQueryable;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.UserTest.Commands
{
    public class UserCommandHandlerTests
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly UserProfile _userProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IApplicationUserService> _appUserServiceMock;
        private readonly UserCommandHandler _handler;
        #endregion

        #region Constructors
        public UserCommandHandlerTests()
        {
            _userProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_userProfile));
            _mapper = new Mapper(configuration);

            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();
            _localizerMock.Setup(l => l[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            _appUserServiceMock = new Mock<IApplicationUserService>();

            _handler = new UserCommandHandler(
                _mapper,
                _userManagerMock.Object,
                _localizerMock.Object,
                _appUserServiceMock.Object
            );
        }
        #endregion

        #region Tests

        #region AddUserCommand Tests

        [Fact]
        public async Task AddUser_ShouldReturnSuccess_WhenServiceReturnsSuccess()
        {
            var cmd = new AddUserCommand { Email = "test@test.com", Password = "Pass1234" };
            var appUser = new ApplicationUser();

            _appUserServiceMock.Setup(s => s.AddUserAsync(It.IsAny<ApplicationUser>(), cmd.Password)).ReturnsAsync("Success");

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }

        [Theory]
        [InlineData("EmailIsExist", SharedResourcesKeys.EmailIsExist)]
        [InlineData("UserNameIsExist", SharedResourcesKeys.UserNameIsExist)]
        [InlineData("ErrorInCreateUser", SharedResourcesKeys.FaildToAddUser)]
        [InlineData("Failed", SharedResourcesKeys.TryToRegisterAgain)]
        public async Task AddUser_ShouldReturnBadRequest_ForKnownErrors(string serviceResult, string expectedKey)
        {
            var cmd = new AddUserCommand();
            var appUser = new ApplicationUser();
            _appUserServiceMock.Setup(s => s.AddUserAsync(It.IsAny<ApplicationUser>(), cmd.Password)).ReturnsAsync(serviceResult);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be(expectedKey);
        }

        #endregion

        #region EditUserCommand Tests

        [Fact]
        public async Task EditUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            var cmd = new EditUserCommand { Id = 1 };
            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task EditUser_ShouldReturnBadRequest_WhenUserNameExists()
        {
            var existingUser = new ApplicationUser { Id = 1 };
            var otherUser = new ApplicationUser { Id = 2, UserName = "SameName" };

            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(existingUser);
            _userManagerMock.Setup(m => m.Users).Returns(new List<ApplicationUser> { otherUser }.AsQueryable().BuildMock());

            var cmd = new EditUserCommand { Id = 1, UserName = "SameName" };
            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be(SharedResourcesKeys.UserNameIsExist);
        }

        #endregion

        #region DeleteUserCommand Tests

        [Fact]
        public async Task DeleteUser_ShouldReturnSuccess_WhenUserDeleted()
        {
            var user = new ApplicationUser { Id = 1 };
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var cmd = new DeleteUserCommand(1);
            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }


        #endregion

        #region ChangeUserPasswordCommand Tests

        [Fact]
        public async Task ChangePassword_ShouldReturnBadRequest_WhenFailed()
        {
            var user = new ApplicationUser { Id = 1 };
            var identityError = new IdentityError { Description = "Password too weak" };
            _userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.ChangePasswordAsync(user, "old", "new"))
                .ReturnsAsync(IdentityResult.Failed(identityError));

            var cmd = new ChangeUserPasswordCommand { Id = 1, CurrentPassword = "old", NewPassword = "new" };
            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Password too weak");
        }

        #endregion

        #endregion

    }
}
