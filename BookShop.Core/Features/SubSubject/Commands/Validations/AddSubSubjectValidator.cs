using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.SubSubject.Commands.Validations
{
    public class AddSubSubjectValidator : AbstractValidator<AddSubSubjectCommand>
    {
        #region Fields
        private readonly ISubSubjectService _subSubjectService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddSubSubjectValidator(ISubSubjectService subSubjectService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _subSubjectService = subSubjectService;
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

            // SubjectId validation 
            RuleFor(b => b.SubjectId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Greater]);
        }
        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name_Ar)
                .MustAsync(async (Key, CancellationToken) => !await _subSubjectService.IsNameArExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
            RuleFor(x => x.Name)
               .MustAsync(async (Key, CancellationToken) => !await _subSubjectService.IsNameExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
            RuleFor(x => x.SubjectId)
               .MustAsync(async (Key, CancellationToken) => await _subSubjectService.IsSubjectIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion

    }
}
