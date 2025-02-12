using AutoMapper;

namespace BookShop.Core.Mapping.Review
{
    public partial class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            AddReviewCommandMapping();
            ReviewCommandMapping();
            EditReviewCommandMapping();
            GetReviewListMapping();
            GetReviewByIdMapping();
            BookReviewMapping();
        }
    }
}
