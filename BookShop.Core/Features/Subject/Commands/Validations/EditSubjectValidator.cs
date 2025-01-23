using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Subject.Commands.Validations
{
    public class EditSubjectValidator : AbstractValidator<EditSubjectCommand>
    {
        #region Fields
        private readonly ISubjectService _subjectService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditSubjectValidator(ISubjectService subjectService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _subjectService = subjectService;
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
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            // Name_Ar validation
            RuleFor(b => b.Name_Ar)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name_Ar)
                .MustAsync(async (model, Key, CancellationToken) => !await _subjectService.IsNameArExistExcludeSelf(Key, model.Id))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.Name)
               .MustAsync(async (model, Key, CancellationToken) => !await _subjectService.IsNameExistExcludeSelf(Key, model.Id))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion

    }
}
