namespace BookShop.Core.Features.Review.Queries.Response_DTO_
{
    public class GetReviewListResponse
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public int BookId { get; set; }
    }
}
