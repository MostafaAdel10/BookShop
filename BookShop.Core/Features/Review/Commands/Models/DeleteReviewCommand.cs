using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Review.Commands.Models
{
    public class DeleteReviewCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteReviewCommand(int id)
        {
            Id = id;
        }
    }
}
