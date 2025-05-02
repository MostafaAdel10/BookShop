using System.Text.Json.Serialization;

namespace BookShop.Service.External.Models
{
    public class VodafoneCashResponseDto
    {
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }

        [JsonPropertyName("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
