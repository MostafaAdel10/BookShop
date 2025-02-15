using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Payment_Methods.Queries.Models;
using BookShop.Core.Features.Payment_Methods.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Payment_Methods.Queries.Handlers
{
    public class Payment_MethodsQueryHandler : ResponseHandler,
            IRequestHandler<GetPayment_MethodsListQuery, Response<List<GetPayment_MethodsListResponse>>>,
            IRequestHandler<GetPayment_MethodsByIdQuery, Response<GetSinglePayment_MethodsResponse>>
    {
        #region Fields
        private readonly IPayment_MethodsService _payment_MethodsService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public Payment_MethodsQueryHandler(IPayment_MethodsService payment_MethodsService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _payment_MethodsService = payment_MethodsService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetPayment_MethodsListResponse>>> Handle(GetPayment_MethodsListQuery request, CancellationToken cancellationToken)
        {
            var payment_MethodsList = await _payment_MethodsService.GetPayment_MethodsListAsync();
            var payment_MethodsListMapper = _mapper.Map<List<GetPayment_MethodsListResponse>>(payment_MethodsList);

            var result = Success(payment_MethodsListMapper);
            result.Meta = new { Count = payment_MethodsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSinglePayment_MethodsResponse>> Handle(GetPayment_MethodsByIdQuery request, CancellationToken cancellationToken)
        {
            var payment_Method = await _payment_MethodsService.GetPayment_MethodByIdAsync(request.Id);

            if (payment_Method == null) return NotFound<GetSinglePayment_MethodsResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSinglePayment_MethodsResponse>(payment_Method);
            return Success(result);
        }
        #endregion
    }
}
