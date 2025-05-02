using BookShop.Service.External.Models;

namespace BookShop.Service.External.Interfaces
{
    public interface IVodafoneCashClient
    {
        /// <summary>
        /// Initiates a Vodafone Cash payment and returns transaction details.
        /// </summary>
        Task<MobileCashInitiationResult> InitiatePaymentAsync(
            int orderId,
            decimal amount,
            string currency,
            int userId);
    }
}

