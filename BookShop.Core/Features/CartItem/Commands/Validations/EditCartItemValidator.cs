using BookShop.Core.Features.CartItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Commands.Validations
{
    public class EditCartItemValidator : AbstractValidator<EditCartItemCommand>
    {
        #region Fields
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IBookService _bookService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditCartItemValidator(IShoppingCartService shoppingCartService, IStringLocalizer<SharedResources> localizer,
            IBookService bookService)
        {
            _shoppingCartService = shoppingCartService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _bookService = bookService;
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.BookId)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.ShoppingCartId)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.ShoppingCartId)
               .MustAsync(async (Key, CancellationToken) => await _shoppingCartService.IsShoppingCartIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);

            RuleFor(x => x.BookId)
               .MustAsync(async (Key, CancellationToken) => await _bookService.IsBookIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}
