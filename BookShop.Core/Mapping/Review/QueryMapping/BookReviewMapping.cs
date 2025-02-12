using BookShop.Core.Features.Review.Commands.Models;

namespace BookShop.Core.Mapping.Review
{
    public partial class ReviewProfile
    {
        public void BookReviewMapping()
        {
            CreateMap<DataAccess.Entities.Review, ReviewCommand>();
        }
    }
}
