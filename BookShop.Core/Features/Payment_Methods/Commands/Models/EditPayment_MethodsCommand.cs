using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Payment_Methods.Commands.Models
{
    public class EditPayment_MethodsCommand : IRequest<Response<Payment_MethodsCommand>>
    {
        public int Id { get; set; }
        public string? Card_Number { get; set; }
        public DateOnly? Expiration_Date { get; set; }
        public bool Is_Default { get; set; } = false;
        public string Name { get; set; }
    }
}
