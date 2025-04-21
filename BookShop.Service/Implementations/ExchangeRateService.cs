using BookShop.Service.Abstract;
using Microsoft.Extensions.Configuration;

namespace BookShop.Service.Implementations
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ExchangeRateService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["ExchangeRateApi:ApiKey"];
        }

        public async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            // هنا هتكتب الكود اللي يجيب سعر الصرف من الAPI
            // مثلا: تستخدم HttpClient للاتصال بالAPI وتحلل الرد
            string url = $"https://api.exchangeratesapi.io/latest?base={fromCurrency}&symbols={toCurrency}&access_key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            // تحليل الـ JSON واستخراج سعر الصرف (هتعدل الكود حسب الAPI اللي بتستخدمه)
            throw new NotImplementedException(); // استبدل دي بالكود الحقيقي
        }
    }
}
