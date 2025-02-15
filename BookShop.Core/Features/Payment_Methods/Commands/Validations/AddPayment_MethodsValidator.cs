using BookShop.Core.Features.Payment_Methods.Commands.Models;
using BookShop.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Payment_Methods.Commands.Validations
{
    public class AddPayment_MethodsValidator : AbstractValidator<AddPayment_MethodsCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddPayment_MethodsValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(card => card.Card_Number)
               .Matches(@"^\d+$").WithMessage(_localizer[SharedResourcesKeys.ContainOnlyDigits])
               .Length(16).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs16]);

            RuleFor(b => b.Is_Default)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
