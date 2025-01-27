namespace BookShop.Core.Features.Discount.Commands.Models
{
    public class DiscountCommand
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Name_Ar { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int? Code { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public bool IsActive { get; set; }
        public decimal Percentage { get; set; }
    }
}
