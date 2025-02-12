using BookShop.Core.Features.Review.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Review.Commands.Validations
{
    public class EditReviewValidator : AbstractValidator<EditReviewCommand>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditReviewValidator(IReviewService reviewService, IStringLocalizer<SharedResources> localizer)
        {
            _reviewService = reviewService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(_localizer[SharedResourcesKeys.RatingBetween1and5]);
        }

        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
