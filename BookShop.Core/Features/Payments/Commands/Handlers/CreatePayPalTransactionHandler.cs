using BookShop.Core.Bases;
using BookShop.Core.Features.Payments.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using Infrastructure.PayPal;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;

namespace Core.Payments.Handlers
{
    public class CreatePayPalTransactionHandler : ResponseHandler,
        IRequestHandler<CreatePayPalTransactionCommand, Response<string>>,
        IRequestHandler<CreateCashOnDeliveryTransactionCommand, Response<string>>,
        IRequestHandler<UpdateCashOnDeliveryStatusCommand, Response<string>>
    {
        private readonly PayPalClient _payPalClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentService _paymentService;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CreatePayPalTransactionHandler> _logger;
        private readonly IConfiguration _config;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreatePayPalTransactionHandler(
            PayPalClient payPalClient,
            ICurrentUserService currentUserService,
            IPaymentService paymentService,
            IExchangeRateService exchangeRateService,
            IMemoryCache cache,
            ILogger<CreatePayPalTransactionHandler> logger,
            IConfiguration config,
            IStringLocalizer<SharedResources> stringLocalizer)
            : base(stringLocalizer)
        {
            _payPalClient = payPalClient;
            _currentUserService = currentUserService;
            _paymentService = paymentService;
            _exchangeRateService = exchangeRateService;
            _cache = cache;
            _logger = logger;
            _config = config;
            _localizer = stringLocalizer;
        }

        /// <summary>
        /// Handles the creation of a PayPal transaction, including currency validation and payment processing.
        /// </summary>
        /// <param name="request">The command containing payment details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response indicating the result of the payment process.</returns>
        public async Task<Response<string>> Handle(CreatePayPalTransactionCommand request, CancellationToken cancellationToken)
        {
            // Validate input data
            if (request.Amount <= 0)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidAmount]);
            }
            if (request.OrderId <= 0)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidOrderId]);
            }

            // Validate supported currencies
            if (request.Currency != "EGP" && request.Currency != "USD")
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.OnlyEGPAndUSDCurrenciesAreSupported]);
            }

            // Convert amount to USD if necessary
            decimal amountToCharge = await ConvertToUSD(request.Amount, request.Currency);

            // Get PayPal client
            var client = _payPalClient.GetClient();
            var currentUserId = _currentUserService.GetUserId();

            // Set up PayPal order request
            var orderRequest = new OrdersCreateRequest();
            orderRequest.Prefer("return=representation");
            orderRequest.RequestBody(new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new System.Collections.Generic.List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD", // Always charge in USD
                            Value = amountToCharge.ToString("F2")
                        }
                    }
                }
            });

            try
            {
                // Execute PayPal order request
                var response = await client.Execute(orderRequest);
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                // Check order status
                if (result.Status == "COMPLETED" || result.Status == "APPROVED")
                {
                    // Create payment record
                    var payment = new Payment
                    {
                        PaymentDate = DateTime.UtcNow,
                        Amount = request.Amount,
                        Currency = request.Currency,
                        PaymentMethod = PaymentMethodType.PayPal,
                        OrderId = request.OrderId,
                        ApplicationUserId = currentUserId
                    };
                    payment.UpdateStatus(PaymentStatus.Completed, result.Id);
                    await _paymentService.AddAsync(payment);
                    return Created<string>(_localizer[SharedResourcesKeys.PaymentProcessedSuccessfully]);
                }
                else if (result.Status == "PENDING")
                {
                    // Create payment record with pending status
                    var payment = new Payment
                    {
                        PaymentDate = DateTime.UtcNow,
                        Amount = request.Amount,
                        Currency = request.Currency,
                        PaymentMethod = PaymentMethodType.PayPal,
                        OrderId = request.OrderId,
                        ApplicationUserId = currentUserId
                    };
                    payment.UpdateStatus(PaymentStatus.Pending, result.Id);
                    await _paymentService.AddAsync(payment);
                    return Success<string>(_localizer[SharedResourcesKeys.PaymentPending]);
                }
                else
                {
                    _logger.LogWarning("PayPal payment failed with status: {Status}", result.Status);
                    return BadRequest<string>(_localizer[SharedResourcesKeys.PaymentNotCompleted]);
                }
            }
            catch (HttpException ex)
            {
                _logger.LogError(ex, "Failed to process PayPal payment for OrderId: {OrderId}", request.OrderId);
                return BadRequest<string>($"{_localizer[SharedResourcesKeys.PaymentProcessingFailed]}: {ex.Message}");
            }
        }

        /// <summary>
        /// Converts the amount to USD if the currency is EGP.
        /// </summary>
        /// <param name="amount">The amount to convert.</param>
        /// <param name="currency">The currency of the amount.</param>
        /// <returns>The amount in USD.</returns>
        private async Task<decimal> ConvertToUSD(decimal amount, string currency)
        {
            if (currency == "USD") return amount;
            if (currency == "EGP")
            {
                var exchangeRate = await GetExchangeRate("EGP", "USD");
                return Math.Round(amount / exchangeRate, 2);
            }
            throw new ArgumentException("Unsupported currency");
        }

        /// <summary>
        /// Gets the exchange rate from cache or service.
        /// </summary>
        /// <param name="fromCurrency">The source currency.</param>
        /// <param name="toCurrency">The target currency.</param>
        /// <returns>The exchange rate.</returns>
        private async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            string cacheKey = $"ExchangeRate_{fromCurrency}_{toCurrency}";
            if (!_cache.TryGetValue(cacheKey, out decimal rate))
            {
                rate = await _exchangeRateService.GetExchangeRate(fromCurrency, toCurrency);
                _cache.Set(cacheKey, rate, TimeSpan.FromHours(1));
            }
            return rate;
        }

        /// <summary>
        /// Handles the creation of a Cash On Delivery transaction.
        /// </summary>
        /// <param name="request">The command containing payment details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<Response<string>> Handle(CreateCashOnDeliveryTransactionCommand request, CancellationToken cancellationToken)
        {
            // Validate input data
            if (request.Amount <= 0)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidAmount]);
            }
            if (request.OrderId <= 0)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.InvalidOrderId]);
            }

            // Check if the currency is supported for COD
            var supportedCurrencies = _config["Payment:CashOnDelivery:SupportedCurrencies"]?.Split(',') ?? new string[] { "EGP" };
            if (!supportedCurrencies.Contains(request.Currency))
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.CODOnlySupportsConfiguredCurrencies]);
            }

            var currentUserId = _currentUserService.GetUserId();

            // Create COD payment record
            var codPayment = new Payment
            {
                PaymentDate = DateTime.UtcNow,
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethod = PaymentMethodType.CashOnDelivery,
                OrderId = request.OrderId,
                ApplicationUserId = currentUserId
            };
            codPayment.UpdateStatus(PaymentStatus.Pending, "CashOnDelivery");
            await _paymentService.AddAsync(codPayment);

            return Created<string>(_localizer[SharedResourcesKeys.OrderPlacedCashOnDelivery]);
        }

        /// <summary>
        /// Handles the update of a Cash On Delivery payment status.
        /// </summary>
        /// <param name="request">The command containing the payment ID and new status.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<Response<string>> Handle(UpdateCashOnDeliveryStatusCommand request, CancellationToken cancellationToken)
        {
            // Get the payment
            var payment = await _paymentService.GetByIdAsync(request.PaymentId);
            if (payment == null || payment.PaymentMethod != PaymentMethodType.CashOnDelivery)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.NotFound]);
            }

            // Check user permissions
            var currentUserId = _currentUserService.GetUserId();
            if (payment.ApplicationUserId != currentUserId)
            {
                return Unauthorized<string>(_localizer[SharedResourcesKeys.UnAuthorized]);
            }

            // Update the payment status
            payment.UpdateStatus(request.NewStatus, payment.TransactionId);
            await _paymentService.EditAsync(payment);

            return Created<string>(_localizer[SharedResourcesKeys.Updated]);
        }
    }
}