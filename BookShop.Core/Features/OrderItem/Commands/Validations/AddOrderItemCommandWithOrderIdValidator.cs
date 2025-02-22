using BookShop.Core.Features.OrderItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.OrderItem.Commands.Validations
{
    public class AddOrderItemCommandWithOrderIdValidator : AbstractValidator<AddOrderItemCommandWithOrderId>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddOrderItemCommandWithOrderIdValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer,
            IBookService bookService, IShipping_MethodService shipping_MethodService)
        {
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
            RuleFor(o => o.BookId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.OrderId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.UserId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.Price)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.Quantity)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.BookId)
                .MustAsync(async (Key, CancellationToken) => await _bookService.IsBookIdExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.OrderId)
                .MustAsync(async (userId, Key, CancellationToken) => await _orderService.IsOrderIdExistWithUserId(Key, userId.UserId))
                .WithMessage(_localizer[SharedResourcesKeys.OrderIdOrUserIdIsNotExist]);
            RuleFor(o => o.Quantity)
                .MustAsync(async (bookId, key, cancellationToken) => await _bookService.IsQuantityGraterThanExist(bookId.BookId, key))
                .WithMessage(_localizer[SharedResourcesKeys.QuantityIsGreater]);
            RuleFor(o => o.Price)
                .MustAsync(async (bookId, key, cancellationToken) => await _bookService.IsPriceTrueExist(bookId.BookId, key))
                .WithMessage(_localizer[SharedResourcesKeys.PriceIsNotTrue]);
            RuleFor(o => o.BookId)
                .MustAsync(async (key, cancellationToken) => await _bookService.IsTheBookInStock(key))
                .WithMessage(_localizer[SharedResourcesKeys.TheBookIsNotAvailable]);
        }
        #endregion
    }
}
