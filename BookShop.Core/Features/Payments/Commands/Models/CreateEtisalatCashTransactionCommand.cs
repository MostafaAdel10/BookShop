using BookShop.Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Features.Payments.Commands.Models
{
    public class CreateEtisalatCashTransactionCommand : IRequest<Response<string>>
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
