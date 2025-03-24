namespace BookShop.Core.Features.CartItem.Commands.Models
{
    public class CartItemCommand
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}