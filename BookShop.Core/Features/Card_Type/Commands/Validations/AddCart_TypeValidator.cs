using BookShop.Core.Features.Card_Type.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Card_Type.Commands.Validations
{
    public class AddCart_TypeValidator : AbstractValidator<AddCart_TypeCommand>
    {
        #region Fields
        private readonly ICart_TypeService _cart_TypeService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddCart_TypeValidator(ICart_TypeService cart_TypeService, IStringLocalizer<SharedResources> localizer)
        {
            _cart_TypeService = cart_TypeService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name)
               .MustAsync(async (Key, CancellationToken) => !await _cart_TypeService.IsNameExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion
    }
}
