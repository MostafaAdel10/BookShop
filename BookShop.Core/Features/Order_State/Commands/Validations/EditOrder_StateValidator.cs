using BookShop.Core.Features.Order_State.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order_State.Commands.Validations
{
    public class EditOrder_StateValidator : AbstractValidator<EditOrder_StateCommand>
    {
        #region Fields
        private readonly IOrder_StateService _order_StateService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditOrder_StateValidator(IOrder_StateService order_StateService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _order_StateService = order_StateService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            // Name_En validation
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);

            // Name_Ar validation
            RuleFor(b => b.Name_Ar)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name_Ar)
                .MustAsync(async (Key, CancellationToken) => !await _order_StateService.IsNameArExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
            RuleFor(x => x.Name)
               .MustAsync(async (Key, CancellationToken) => !await _order_StateService.IsNameExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion
    }
}
