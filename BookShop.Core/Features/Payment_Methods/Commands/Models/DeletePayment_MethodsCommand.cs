using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Payment_Methods.Commands.Models
{
    public class DeletePayment_MethodsCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeletePayment_MethodsCommand(int id)
        {
            Id = id;
        }
    }
}
