using BookShop.Core.Features.Emails.Commands.Handlers;
using BookShop.Core.Features.Emails.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.CoreTests.EmailTest.Commands
{
    public class EmailsCommandHandlerTests
    {
        private readonly Mock<IEmailsService> _emailsServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly EmailsCommandHandler _handler;

        public EmailsCommandHandlerTests()
        {
            _emailsServiceMock = new();
            _localizerMock = new();

            _localizerMock.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));

            _handler = new EmailsCommandHandler(
                _localizerMock.Object,
                _emailsServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenEmailIsSentSuccessfully()
        {
            // Arrange
            var command = new SendEmailCommand
            {
                Email = "test@example.com",
                Message = "Hello",
                Reason = "Test"
            };

            _emailsServiceMock.Setup(s => s.SendEmail(command.Email, command.Message, command.Reason))
                .ReturnsAsync("Success");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be("");
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenEmailSendingFails()
        {
            // Arrange
            var command = new SendEmailCommand
            {
                Email = "test@example.com",
                Message = "Hello",
                Reason = "Test"
            };

            _emailsServiceMock.Setup(s => s.SendEmail(command.Email, command.Message, command.Reason))
                .ReturnsAsync("Error");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result.Message.Should().Be(SharedResourcesKeys.SendEmailFailed);
        }
    }
}
