using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddOrderValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer)
        {
            _orderService = orderService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(o => o.OrderDate)
            .NotEmpty()
            .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.TotalAmount)
                .InclusiveBetween(0.01m, 1_000_000m)
                .WithMessage(_localizer[SharedResourcesKeys.TotalAmount]);

            RuleFor(o => o.TrackingNumber)
                .MaximumLength(15)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs15]);

            RuleFor(o => o.ShippingAddress)
                .MaximumLength(1500)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs1500]);

            RuleFor(o => o.OrderItems)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.ShippingMethodId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.PaymentMethodId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.UserId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.OrderStateId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
