using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Features.OrderItem.Commands.Models
{
    public class EditOrderItemCommand
    {
        [Required]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public int BookId { get; set; }
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public int? Tax { get; set; }
    }
}
