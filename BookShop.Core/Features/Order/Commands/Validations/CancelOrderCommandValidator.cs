using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Validations
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CancelOrderCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(o => o.OrderId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

        }
        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
