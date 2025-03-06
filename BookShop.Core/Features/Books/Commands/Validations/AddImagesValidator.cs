using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Books.Commands.Validations
{
    public class AddImagesValidator : AbstractValidator<AddImagesCommand>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddImagesValidator(IBookService bookService, IStringLocalizer<SharedResources> localizer)
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
            RuleFor(b => b.Id)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.Images)
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
