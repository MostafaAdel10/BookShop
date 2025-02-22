using BookShop.Core.Features.OrderItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.OrderItem.Commands.Validations
{
    public class DeleteOrderItemCommandWithOrderIdValidator : AbstractValidator<DeleteOrderItemCommandWithOrderId>
    {
        #region Fields
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public DeleteOrderItemCommandWithOrderIdValidator(IOrderService orderService, IOrderItemService orderItemService,
            IStringLocalizer<SharedResources> localizer,
            IBookService bookService, IShipping_MethodService shipping_MethodService)
        {
            _orderItemService = orderItemService;
            _orderService = orderService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _bookService = bookService;
            _shipping_MethodService = shipping_MethodService;
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(o => o.Id)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.OrderId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.UserId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Id)
                .MustAsync(async (Key, CancellationToken) => await _orderItemService.IsOrderItemIdExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.OrderId)
                .MustAsync(async (userId, Key, CancellationToken) => await _orderService.IsOrderIdExistWithUserId(Key, userId.UserId))
                .WithMessage(_localizer[SharedResourcesKeys.OrderIdOrUserIdIsNotExist]);
        }
        #endregion
    }
}
