namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewsListByBookIdResponse
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public int BookId { get; set; }
        public string UserName { get; set; }
    }
}
