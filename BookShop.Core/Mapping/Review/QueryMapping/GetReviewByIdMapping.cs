using BookShop.Core.Features.Review.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.Review
{
    public partial class ReviewProfile
    {
        public void GetReviewByIdMapping()
        {
            CreateMap<DataAccess.Entities.Review, GetSingleReviewResponse>();
        }
    }
}
