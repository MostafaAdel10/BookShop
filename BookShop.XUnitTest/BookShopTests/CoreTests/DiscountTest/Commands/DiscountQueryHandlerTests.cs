using AutoMapper;
using BookShop.Core.Features.Discount.Commands.Handlers;
using BookShop.Core.Features.Discount.Commands.Models;
using BookShop.Core.Mapping.Discounts;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.XUnitTest.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.DiscountTest.Commands
{
    public class DiscountQueryHandlerTests
    {
        #region Fields

        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly Mock<IBook_DiscountService> _bookDiscountServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly IMapper _mapper;
        private readonly DiscountProfile _discountProfile;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;

        private readonly DiscountCommandHandler _handler;

        #endregion

        #region Constructor

        public DiscountQueryHandlerTests()
        {
            _discountServiceMock = new Mock<IDiscountService>();
            _bookDiscountServiceMock = new Mock<IBook_DiscountService>();
            _fileServiceMock = new Mock<IFileService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();
            _discountProfile = new();

            var configuration = new MapperConfiguration(c => c.AddProfile(_discountProfile));
            _mapper = new Mapper(configuration);

            _handler = new DiscountCommandHandler(
                _discountServiceMock.Object,
                _mapper,
                _bookDiscountServiceMock.Object,
                _fileServiceMock.Object,
                _localizerMock.Object);
        }

        #endregion

        #region Handel Functions Test

        #region AddDiscountCommand Tests

        [Fact]
        public async Task Handle_AddDiscountCommand_WithValidImage_ShouldReturnCreated()
        {
            var ImageData = TestFileHelper.CreateTestFormFile();

            var command = new AddDiscountCommand { ImageData = ImageData, Name = "Test", Name_Ar = "اختبار", Code = 123, Start_date = DateTime.Now, End_date = DateTime.Now.AddDays(1), IsActive = true, Percentage = 10 };

            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("url");
            _discountServiceMock.Setup(x => x.AddAsync(It.IsAny<Discount>())).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_AddDiscountCommand_ImageUploadFails_ShouldReturnBadRequest()
        {
            var ImageData = TestFileHelper.CreateTestFormFile();

            var command = new AddDiscountCommand { ImageData = ImageData };
            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync(string.Empty);

            var localizedString = new LocalizedString("FailedToUploadImage", "Failed to upload image");
            _localizerMock.Setup(x => x[SharedResourcesKeys.FailedToUploadImage]).Returns(localizedString);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be(localizedString.Value);
        }

        [Fact]
        public async Task Handle_AddDiscountCommand_AddFails_ShouldReturnBadRequest()
        {
            var command = new AddDiscountCommand();
            _fileServiceMock.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("url");

            _discountServiceMock.Setup(x => x.AddAsync(It.IsAny<Discount>())).ReturnsAsync("Fail");

            var localizedString = new LocalizedString("FailedToAdd", "Failed to add");
            _localizerMock.Setup(x => x[SharedResourcesKeys.FailedToAdd]).Returns(localizedString);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be(localizedString.Value);
        }

        #endregion

        #region EditDiscountCommand Tests
        [Fact]
        public async Task Handle_EditDiscountCommand_Valid_ShouldReturnSuccess()
        {
            var command = new EditDiscountCommand { Id = 1 };
            var existingDiscount = new Discount();
            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync(existingDiscount);
            _discountServiceMock.Setup(x => x.EditAsync(existingDiscount)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_EditDiscountCommand_NotFound_ShouldReturnNotFound()
        {
            var command = new EditDiscountCommand { Id = 1 };
            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync((Discount)null!);

            var result = await _handler.Handle(command, CancellationToken.None);
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_EditDiscountCommand_UpdateFails_ShouldReturnBadRequest()
        {
            var command = new EditDiscountCommand { Id = 1 };
            var discount = new Discount();
            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync(discount);
            _discountServiceMock.Setup(x => x.EditAsync(discount)).ReturnsAsync("Fail");
            _localizerMock.Setup(x => x[SharedResourcesKeys.FailedToUpdate]).Returns(new LocalizedString("FailedToUpdate", "Update failed"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Update failed");
        }
        #endregion

        #region DeleteDiscountCommand Tests
        [Fact]
        public async Task Handle_DeleteDiscountCommand_Valid_ShouldReturnDeleted()
        {
            var command = new DeleteDiscountCommand(1);
            var discount = new Discount { Id = 1, ImageUrl = "url" };

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync(discount);
            _bookDiscountServiceMock.Setup(x => x.IsDiscountRelatedWithBook(discount.Id)).ReturnsAsync(false);
            _fileServiceMock.Setup(x => x.DeleteImage(discount.ImageUrl)).Returns(true);
            _discountServiceMock.Setup(x => x.DeleteAsync(discount)).ReturnsAsync("Success");

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_DeleteDiscountCommand_NotFound_ShouldReturnNotFound()
        {
            var command = new DeleteDiscountCommand(1);
            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync((Discount)null!);

            var result = await _handler.Handle(command, CancellationToken.None);
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_DeleteDiscountCommand_RelatedToBooks_ShouldReturnUnprocessable()
        {
            var command = new DeleteDiscountCommand(1);
            var discount = new Discount { Id = 1 };

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync(discount);
            _bookDiscountServiceMock.Setup(x => x.IsDiscountRelatedWithBook(discount.Id)).ReturnsAsync(true);
            _localizerMock.Setup(x => x[SharedResourcesKeys.ReferencedInAnotherTable]).Returns(new LocalizedString("Referenced", "Referenced"));

            var result = await _handler.Handle(command, CancellationToken.None);
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }

        [Fact]
        public async Task Handle_DeleteDiscountCommand_DeleteFails_ShouldReturnBadRequest()
        {
            var command = new DeleteDiscountCommand(1);
            var discount = new Discount { Id = 1, ImageUrl = "url" };

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync(discount);
            _bookDiscountServiceMock.Setup(x => x.IsDiscountRelatedWithBook(discount.Id)).ReturnsAsync(false);
            _fileServiceMock.Setup(x => x.DeleteImage(discount.ImageUrl)).Returns(true);
            _discountServiceMock.Setup(x => x.DeleteAsync(discount)).ReturnsAsync("Fail");
            _localizerMock.Setup(x => x[SharedResourcesKeys.FailedToDelete]).Returns(new LocalizedString("FailedToDelete", "Delete failed"));

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Delete failed");
        }

        [Fact]
        public async Task Handle_DeleteDiscountCommand_ImageDeleteFails_ShouldReturnBadRequest()
        {
            var command = new DeleteDiscountCommand(1);
            var discount = new Discount { Id = 1, ImageUrl = "url" };

            _discountServiceMock.Setup(x => x.GetDiscountByIdAsync(command.Id)).ReturnsAsync(discount);
            _bookDiscountServiceMock.Setup(x => x.IsDiscountRelatedWithBook(discount.Id)).ReturnsAsync(false);
            _fileServiceMock.Setup(x => x.DeleteImage(discount.ImageUrl)).Returns(false);
            _localizerMock.Setup(x => x[SharedResourcesKeys.DeletedFailed]).Returns(new LocalizedString("DeletedFailed", "Image delete failed"));

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Image delete failed");
        }
        #endregion

        #endregion
    }
}
