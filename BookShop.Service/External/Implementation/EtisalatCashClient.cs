using BookShop.Service.External.Interfaces;
using BookShop.Service.External.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BookShop.Service.External.Implementation
{
    public class EtisalatCashClient : IEtisalatCashClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EtisalatCashClient> _logger;
        private readonly string _initiateUrl;

        public EtisalatCashClient(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<EtisalatCashClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _initiateUrl = configuration["ExternalPayments:EtisalatCash:InitiateUrl"];
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
                    _logger.LogError("EtisalatCash API error: {Error}", error);
                    return new MobileCashInitiationResult(false, null, error, null);
                }

                var dto = await response.Content.ReadFromJsonAsync<
                    EtisalatCashResponseDto>();
                return new MobileCashInitiationResult(
                    true,
                    dto.TransactionId,
                    null,
                    dto.RedirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception calling EtisalatCash API");
                return new MobileCashInitiationResult(false, null, ex.Message, null);
            }
        }
    }
}