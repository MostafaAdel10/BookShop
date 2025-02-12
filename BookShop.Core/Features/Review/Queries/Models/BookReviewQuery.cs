namespace BookShop.Core.Features.Review.Queries.Models
{
    public class BookReviewQuery
    {
        public int Rating { get; set; }
        public string? Content { get; set; }
        public string Name { get; set; }
        public string User_Name { get; set; }
    }
}
