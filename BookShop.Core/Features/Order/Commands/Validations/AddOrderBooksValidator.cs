using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class AddOrderBooksValidator : AbstractValidator<OrderBooks>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddOrderBooksValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer,
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
        }
        #endregion
    }
}
