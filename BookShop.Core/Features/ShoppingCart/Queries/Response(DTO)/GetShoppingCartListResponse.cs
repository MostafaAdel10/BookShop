namespace BookShop.Core.Features.ShoppingCart.Queries.Response_DTO_
{
    public class GetShoppingCartListResponse
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
