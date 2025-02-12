using BookShop.Core.Features.Review.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.Review
{
    public partial class ReviewProfile
    {
        public void GetReviewListMapping()
        {
            CreateMap<DataAccess.Entities.Review, GetReviewListResponse>();
        }
    }
}
