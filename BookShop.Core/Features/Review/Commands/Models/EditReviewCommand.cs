using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Review.Commands.Models
{
    public class EditReviewCommand : IRequest<Response<ReviewCommand>>
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
    }
}
