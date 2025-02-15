namespace BookShop.Core.Features.CartItem.Queries.Response_DTO_
{
    public class GetSingleCartItemResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
