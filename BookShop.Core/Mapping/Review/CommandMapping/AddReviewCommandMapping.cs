using BookShop.Core.Features.Review.Commands.Models;

namespace BookShop.Core.Mapping.Review
{
    public partial class ReviewProfile
    {
        public void AddReviewCommandMapping()
        {
            CreateMap<AddReviewCommand, DataAccess.Entities.Review>();
        }
    }
}
