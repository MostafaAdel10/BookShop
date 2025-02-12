using BookShop.Core.Features.Review.Queries.Models;

namespace BookShop.Core.Mapping.Review
{
    public partial class ReviewProfile
    {
        public void ReviewCommandMapping()
        {
            CreateMap<BookReviewQuery, DataAccess.Entities.Review>();
        }
    }
}
