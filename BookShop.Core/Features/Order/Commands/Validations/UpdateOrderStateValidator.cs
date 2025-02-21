using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class UpdateOrderStateValidator : AbstractValidator<UpdateOrderStateCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IOrder_StateService _orderStateService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateOrderStateValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer,
            IOrder_StateService orderStateService)
        {
            _orderService = orderService;
            _localizer = localizer;
            _orderStateService = orderStateService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(o => o.OrderId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.OrderStateId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.OrderId)
               .MustAsync(async (Key, CancellationToken) => await _orderService.IsOrderIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);

            RuleFor(x => x.OrderStateId)
               .MustAsync(async (Key, CancellationToken) => await _orderStateService.IsOrderStateIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}
