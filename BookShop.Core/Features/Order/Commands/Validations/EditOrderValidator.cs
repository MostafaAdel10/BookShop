using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class EditOrderValidator : AbstractValidator<EditOrderCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrder_StateService _order_StateService;
        private readonly IBookService _bookService;
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditOrderValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer,
            IBookService bookService, IShipping_MethodService shipping_MethodService,
            IApplicationUserService applicationUserService, IOrder_StateService order_StateService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _bookService = bookService;
            _shipping_MethodService = shipping_MethodService;
            _applicationUserService = applicationUserService;
            _order_StateService = order_StateService;
            _orderItemService = orderItemService;
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Code)
            .InclusiveBetween(1000, int.MaxValue)
            .When(x => x.Code.HasValue) // يتحقق فقط إذا كانت القيمة غير null
            .WithMessage(_localizer[SharedResourcesKeys.MustBeAtLeast1000]);

            RuleFor(b => b.ShippingAddress)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(1500).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs1500]);

            RuleFor(b => b.TrackingNumber)
                .MaximumLength(15).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs15])
                .Matches(@"^\d{15}$").WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs15]);

            RuleFor(o => o.ShippingMethodId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.Id)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.OrderStateId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.ShippingMethodId)
               .MustAsync(async (Key, CancellationToken) => await _shipping_MethodService.IsShippingMethodIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.OrderStateId)
               .MustAsync(async (Key, CancellationToken) => await _order_StateService.IsOrderStateIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.Id)
               .MustAsync(async (Key, CancellationToken) => await _orderService.IsOrderIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}
