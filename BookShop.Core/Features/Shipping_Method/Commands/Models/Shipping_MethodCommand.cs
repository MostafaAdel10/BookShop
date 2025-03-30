namespace BookShop.Core.Features.Shipping_Method.Commands.Models
{
    public class Shipping_MethodCommand
    {
        public int Id { get; set; }
        public string Method_Name { get; set; }
        public decimal Cost { get; set; }
        public int DeliveryDurationInDays { get; set; }
    }
}
