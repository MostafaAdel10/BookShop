namespace BookShop.Core.Features.Order.Queries.Response_DTO_
{
    public record OrderItemQuery
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; } = string.Empty;
    }
}
