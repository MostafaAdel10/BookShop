using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class EditOrderCommandValidator : AbstractValidator<EditOrderCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditOrderCommandValidator(IOrderService orderService, IShipping_MethodService shipping_MethodService,
            IStringLocalizer<SharedResources> localizer)
        {
            _orderService = orderService;
            _shipping_MethodService = shipping_MethodService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.ShippingMethodId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.PaymentMethodId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);
            RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(150)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);
            RuleFor(x => x.City).NotEmpty().MaximumLength(50)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
            RuleFor(x => x.State).NotEmpty().MaximumLength(50)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs20]);
            RuleFor(x => x.Country).NotEmpty().MaximumLength(50)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?\d{10,15}$").WithMessage(_localizer[SharedResourcesKeys.InvalidPhone]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.OrderId)
               .MustAsync(async (Key, CancellationToken) => await _orderService.IsOrderIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.ShippingMethodId)
               .MustAsync(async (Key, CancellationToken) => await _shipping_MethodService.IsShippingMethodIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}
