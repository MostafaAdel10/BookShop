namespace BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_
{
    public class GetSingleShipping_MethodResponse
    {
        public int Id { get; set; }
        public string Method_Name { get; set; }
        public decimal Cost { get; set; }
        public int DeliveryDurationInDays { get; set; }
    }
}
