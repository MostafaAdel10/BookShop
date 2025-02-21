using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class AddOrderAPIValidator : AbstractValidator<AddOrderCommandAPI>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddOrderAPIValidator(IOrderService orderService, IStringLocalizer<SharedResources> localizer,
            IBookService bookService, IShipping_MethodService shipping_MethodService, IApplicationUserService applicationUserService)
        {
            _orderService = orderService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _bookService = bookService;
            _shipping_MethodService = shipping_MethodService;
            _applicationUserService = applicationUserService;
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(o => o.TotalAmount)
                .InclusiveBetween(0.01m, 1_000_000m)
                .WithMessage(_localizer[SharedResourcesKeys.TotalAmount]);

            RuleFor(o => o.Books)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.ShippingMethodId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(o => o.UserId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(b => b.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs1500]);

            RuleFor(b => b.PhoneNumber)
                .MaximumLength(15).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs15])
                .Matches(@"^\d{15}$").WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs15]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleForEach(o => o.Books)
                .MustAsync(async (Key, cancellationToken) => await _bookService.IsBookIdExist(Key.BookId))
                .WithMessage(_localizer[SharedResourcesKeys.BookIsNotExist]);

            RuleForEach(o => o.Books)
                .MustAsync(async (key, cancellationToken) => await _bookService.IsQuantityGraterThanExist(key.BookId, key.Quantity))
                .WithMessage(_localizer[SharedResourcesKeys.QuantityIsGreater]);
            RuleForEach(o => o.Books)
                .MustAsync(async (key, cancellationToken) => await _bookService.IsTheBookInStock(key.BookId))
                .WithMessage(_localizer[SharedResourcesKeys.TheBookIsNotAvailable]);

            RuleForEach(o => o.Books)
                .MustAsync(async (key, cancellationToken) => await _bookService.IsPriceTrueExist(key.BookId, key.Price))
                .WithMessage(_localizer[SharedResourcesKeys.PriceIsNotTrue]);

            RuleFor(x => x.ShippingMethodId)
               .MustAsync(async (Key, CancellationToken) => await _shipping_MethodService.IsShippingMethodIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);

            RuleFor(x => x.UserId)
               .MustAsync(async (Key, CancellationToken) => await _applicationUserService.IsUserIdIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}
