using BookShop.Core.Features.Payments.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Payments.Commands.Validations
{
    public class CreateVodafoneCashTransactionCommandValidator : AbstractValidator<CreateVodafoneCashTransactionCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateVodafoneCashTransactionCommandValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _orderService = orderService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.OrderId)
           .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Greater]);

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Greater]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.OrderId)
                .MustAsync(async (Key, CancellationToken) => await _orderService.IsOrderIdExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}
