using BookShop.Core.Features.ShoppingCart.Commands.Models;
using BookShop.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.ShoppingCart.Commands.Validations
{
    public class EditShoppingCartValidator : AbstractValidator<EditShoppingCartCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditShoppingCartValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.ApplicationUserId)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
