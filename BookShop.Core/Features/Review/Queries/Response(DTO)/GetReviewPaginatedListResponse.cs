namespace BookShop.Core.Features.Review.Queries.Response_DTO_
{
    public class GetReviewPaginatedListResponse
    {
        public GetReviewPaginatedListResponse(int id, int rating, string? content, int bookId)
        {
            Id = id;
            Rating = rating;
            Content = content;
            BookId = bookId;
        }
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public int BookId { get; set; }
    }
}
