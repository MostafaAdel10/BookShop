using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Review.Commands.Models
{
    public class AddReviewCommand : IRequest<Response<ReviewCommand>>
    {
        public int Rating { get; set; }
        public string? Content { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
}
