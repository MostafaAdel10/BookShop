namespace BookShop.Service.Abstract
{
    public interface IExchangeRateService
    {
        public Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency);
    }
}
