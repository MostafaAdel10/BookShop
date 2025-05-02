namespace BookShop.Service.External.Models
{
    public class MobileCashInitiationResult
    {
        public bool IsSuccess { get; init; }
        public string? TransactionId { get; init; }
        public string? ErrorMessage { get; init; }
        public string? RedirectUrl { get; init; }

        public MobileCashInitiationResult(bool isSuccess, string? transactionId, string? errorMessage, string? redirectUrl)
        {
            IsSuccess = isSuccess;
            TransactionId = transactionId;
            ErrorMessage = errorMessage;
            RedirectUrl = redirectUrl;
        }
    }
}
