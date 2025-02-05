namespace BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_
{
    public class GetShipping_MethodListResponse
    {
        public int Id { get; set; }
        public string Method_Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime Estimated_Delivery_Time { get; set; }
    }
}
