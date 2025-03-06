using BookShop.Core.Features.Discount.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Discount.Commands.Validations
{
    public class EditDiscountValidator : AbstractValidator<EditDiscountCommand>
    {
        #region Fields
        private readonly IDiscountService _discountService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditDiscountValidator(IDiscountService discountService, IStringLocalizer<SharedResources> localizer)
        {
            _discountService = discountService;
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

            RuleFor(x => x.Name_Ar)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.Code)
           .InclusiveBetween(200, 1000).WithMessage(_localizer[SharedResourcesKeys.between200and1000]);

            RuleFor(x => x.Start_date)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.End_date)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThan(x => x.Start_date).WithMessage(_localizer[SharedResourcesKeys.AfterStartDate]);

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Percentage)
                .InclusiveBetween(0, 100).WithMessage(_localizer[SharedResourcesKeys.between0and100]);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(300).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs300]);

            RuleFor(x => x.ImageData)
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name_Ar)
                .MustAsync(async (model, Key, CancellationToken) => !await _discountService.IsNameArExistExcludeSelf(Key, model.Id))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
            RuleFor(x => x.Name)
               .MustAsync(async (model, Key, CancellationToken) => !await _discountService.IsNameExistExcludeSelf(Key, model.Id))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
            RuleFor(x => x.Code)
               .MustAsync(async (model, Key, CancellationToken) => !await _discountService.IsCodeExistExcludeSelf(Key, model.Id))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist])
               .When(x => x.Code != 0);
        }
        #endregion
    }
}
