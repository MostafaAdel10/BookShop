using BookShop.Core.Bases;
using BookShop.Core.Features.Payments.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using BookShop.Service.External.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Payments.Handlers
{
    public class CreateVodafoneCashTransactionHandler
        : ResponseHandler, IRequestHandler<CreateVodafoneCashTransactionCommand, Response<string>>
    {
        private readonly IVodafoneCashClient _walletClient;
        private readonly ICurrentUserService _currentUser;
        private readonly IPaymentService _paymentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreateVodafoneCashTransactionHandler(
            IVodafoneCashClient walletClient,
            ICurrentUserService currentUser,
            IPaymentService paymentService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _walletClient = walletClient;
            _currentUser = currentUser;
            _paymentService = paymentService;
            _localizer = stringLocalizer;
        }

        public async Task<Response<string>> Handle(CreateVodafoneCashTransactionCommand request, CancellationToken cancellationToken)
        {
            var result = await _walletClient.InitiatePaymentAsync(
                request.OrderId,
                request.Amount,
                "EGP",
                _currentUser.GetUserId());

            if (!result.IsSuccess)
            {
                //_logger.LogError("VodafoneCash initiation failed: {Error}", result.ErrorMessage);
                return BadRequest<string>(_localizer[SharedResourcesKeys.PaymentProcessingFailed]);
            }

            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = PaymentMethodType.VodafoneCash,
                ApplicationUserId = _currentUser.GetUserId()
            };
            payment.UpdateStatus(PaymentStatus.Pending, result.TransactionId);
            await _paymentService.AddAsync(payment);

            return Success<string>(result.RedirectUrl);
        }
    }
}