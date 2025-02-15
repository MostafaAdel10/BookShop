namespace BookShop.Core.Features.ShoppingCart.Commands.Models
{
    public class ShoppingCartCommand
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
