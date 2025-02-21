namespace BookShop.Core.Features.OrderItem.Commands.Models
{
    public record OrderItemCommand
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Tax { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; } = string.Empty;
    }
}
