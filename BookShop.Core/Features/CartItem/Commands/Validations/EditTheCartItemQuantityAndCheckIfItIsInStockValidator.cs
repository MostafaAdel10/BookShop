using BookShop.Core.Features.CartItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Commands.Validations
{
    public class EditTheCartItemQuantityAndCheckIfItIsInStockValidator : AbstractValidator<EditTheCartItemQuantityAndCheckIfItIsInStockCommand>
    {
        #region Fields
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IBookService _bookService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditTheCartItemQuantityAndCheckIfItIsInStockValidator(IShoppingCartService shoppingCartService, IStringLocalizer<SharedResources> localizer,
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
            RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}