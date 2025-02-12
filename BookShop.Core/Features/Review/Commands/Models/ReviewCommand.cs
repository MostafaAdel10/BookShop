namespace BookShop.Core.Features.Review.Commands.Models
{
    public class ReviewCommand
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public int BookId { get; set; }
    }
}
