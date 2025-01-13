using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Service.Abstract;
using FluentValidation;

namespace BookShop.Core.Features.Books.Commands.Validations
{
    public class EditBookValidator : AbstractValidator<EditBookCommand>
    {
        #region Fields
        private readonly IBookService _bookService;
        #endregion

        #region Constructors
        public EditBookValidator(IBookService bookService)
        {
            _bookService = bookService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            // Title validation
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(300).WithMessage("Title must not exceed 300 characters.");

            // Description validation (optional, no rules added)
            RuleFor(b => b.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters."); // Example if you want to limit it

            // ISBN13 validation
            RuleFor(b => b.ISBN13)
                .NotEmpty().WithMessage("ISBN must not be empty.")
                .NotNull().WithMessage("ISBN must not be null.")
                .MaximumLength(13).WithMessage("ISBN must be 13 digits.")
                .Matches(@"^\d{13}$").WithMessage("ISBN must contain exactly 13 digits.");

            // Author validation
            RuleFor(b => b.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100).WithMessage("Author must not exceed 100 characters.");

            // Price validation
            RuleFor(b => b.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number.");

            // PriceAfterDiscount validation (optional field)
            //RuleFor(b => b.PriceAfterDiscount)
            //    .GreaterThanOrEqualTo(0).WithMessage("PriceAfterDiscount must be a positive number.")
            //    .When(b => b.PriceAfterDiscount.HasValue).WithMessage("PriceAfterDiscount is optional but must be positive if provided.");

            // Publisher validation
            RuleFor(b => b.Publisher)
                .NotEmpty().WithMessage("Publisher is required.")
                .MaximumLength(100).WithMessage("Publisher must not exceed 100 characters.");

            // PublicationDate validation (optional, can add more specific rules if needed)
            RuleFor(b => b.PublicationDate)
                .NotEmpty().WithMessage("Publication date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date must be in the past.");

            // Unit_Instock validation
            RuleFor(b => b.Unit_Instock)
                .GreaterThanOrEqualTo(0).WithMessage("Unit In Stock must be a positive number.");

            // Image_url validation
            RuleFor(b => b.Image_url)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(300).WithMessage("Image URL must not exceed 300 characters.");

            // IsActive validation (boolean, no specific rules needed unless custom logic is required)

            // SubjectId validation (if it's required or optional)
            RuleFor(b => b.SubjectId)
                .NotEmpty().WithMessage("SubjectId is required.")
                .GreaterThan(0).WithMessage("SubjectId must be greater than 0.");

            // SubSubjectId validation (if it's required or optional)
            RuleFor(b => b.SubSubjectId)
                .NotEmpty().WithMessage("SubSubjectId is required.")
                .GreaterThan(0).WithMessage("SubSubjectId must be greater than 0.");
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(b => b.ISBN13)
                .MustAsync(async (model, key, CancellationToken) => !await _bookService.IsISBNExistExcludeSelf(key, model.Id))
                .WithMessage("ISBN Is Exist.");
        }
        #endregion
    }
}
