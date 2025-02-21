using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Features.OrderItem.Commands.Models
{
    public class AddOrderItemCommand
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public int BookId { get; set; }
        [Required]
        [Range(0, 100)]
        public int Tax { get; set; }
    }
}
