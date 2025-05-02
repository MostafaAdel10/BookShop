using BookShop.Service.External.Interfaces;
using BookShop.Service.External.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BookShop.Service.External.Implementation
{
    public class VodafoneCashClient : IVodafoneCashClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VodafoneCashClient> _logger;
        private readonly string _initiateUrl;

        public VodafoneCashClient(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<VodafoneCashClient> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _initiateUrl = configuration["ExternalPayments:VodafoneCash:InitiateUrl"];
        }

        public async Task<MobileCashInitiationResult> InitiatePaymentAsync(int orderId, decimal amount, string currency, int userId)
        {
            try
            {
                var payload = new { orderId, amount, currency, userId };
                var response = await _httpClient.PostAsJsonAsync(_initiateUrl, payload);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("VodafoneCash API error: {Error}", error);
                    return new MobileCashInitiationResult(false, null, error, null);
                }

                var dto = await response.Content.ReadFromJsonAsync<
                    VodafoneCashResponseDto>();
                return new MobileCashInitiationResult(
                    true,
                    dto.TransactionId,
                    null,
                    dto.RedirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception calling VodafoneCash API");
                return new MobileCashInitiationResult(false, null, ex.Message, null);
            }
        }
    }
}