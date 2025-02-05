using BookShop.Core.Features.Shipping_Method.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Shipping_Method.Commands.Validations
{
    public class EditShipping_MethodValidator : AbstractValidator<EditShipping_MethodCommand>
    {
        #region Fields
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditShipping_MethodValidator(IShipping_MethodService shipping_MethodService, IStringLocalizer<SharedResources> localizer)
        {
            _shipping_MethodService = shipping_MethodService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Method_Name)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Method_Name)
               .MustAsync(async (modle, Key, CancellationToken) => !await _shipping_MethodService.IsNameExistExcludeSelf(Key, modle.Id))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion
    }
}
