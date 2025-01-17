using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Books.Commands.Validations
{
    public class AddBookValidator : AbstractValidator<AddBookCommand>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddBookValidator(IBookService bookService, IStringLocalizer<SharedResources> localizer)
        {
            _bookService = bookService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            // Title validation
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(300).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs300]);

            // ISBN13 validation
            RuleFor(b => b.ISBN13)
                .MaximumLength(13).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs13])
                .Matches(@"^\d{13}$").WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs13]);

            // ISBN10 validation
            RuleFor(b => b.ISBN10)
                .MaximumLength(10).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs10])
                .Matches(@"^\d{10}$").WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs10]);

            // Author validation
            RuleFor(b => b.Author)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            // Price validation
            RuleFor(b => b.Price)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.positive]);

            // Publisher validation
            RuleFor(b => b.Publisher)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            // PublicationDate validation (optional, can add more specific rules if needed)
            RuleFor(b => b.PublicationDate)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .LessThanOrEqualTo(DateTime.Now).WithMessage(_localizer[SharedResourcesKeys.DateTime]);

            // Unit_Instock validation
            RuleFor(b => b.Unit_Instock)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.positive]);

            // Image_url validation
            RuleFor(b => b.Image_url)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoImage])
                .MaximumLength(300).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs300]);

            // SubjectId validation 
            RuleFor(b => b.SubjectId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Greater]);

            // SubSubjectId validation 
            RuleFor(b => b.SubSubjectId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Greater]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(b => b.ISBN13)
                .MustAsync(async (key, CancellationToken) => !await _bookService.IsISBNExist(key))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }
        #endregion

    }
}
