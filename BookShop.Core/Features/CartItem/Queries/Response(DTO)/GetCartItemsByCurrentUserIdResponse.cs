namespace BookShop.Core.Features.CartItem.Queries.Response_DTO_
{
    public class GetCartItemsByCurrentUserIdResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
