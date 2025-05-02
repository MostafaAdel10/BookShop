using BookShop.Core.Features.Payments.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Payments.Commands.Validations
{
    public class UpdateCashOnDeliveryStatusCommandValidator : AbstractValidator<UpdateCashOnDeliveryStatusCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateCashOnDeliveryStatusCommandValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer)
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
            RuleFor(x => x.PaymentId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Greater]);

            RuleFor(x => x.NewStatus)
                .Must(s => s == PaymentStatus.Completed || s == PaymentStatus.Failed)
                .WithMessage(_localizer[SharedResourcesKeys.NewStatus]);
        }
        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
