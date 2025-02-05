using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Shipping_Method.Queries.Models;
using BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Shipping_Method.Queries.Handlers
{
    public class Shipping_MethodQueryHandler : ResponseHandler,
            IRequestHandler<GetShipping_MethodListQuery, Response<List<GetShipping_MethodListResponse>>>,
            IRequestHandler<GetShipping_MethodByIdQuery, Response<GetSingleShipping_MethodResponse>>
    {
        #region Fields
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public Shipping_MethodQueryHandler(IShipping_MethodService shipping_MethodService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _shipping_MethodService = shipping_MethodService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetShipping_MethodListResponse>>> Handle(GetShipping_MethodListQuery request, CancellationToken cancellationToken)
        {
            var shipping_MethodsList = await _shipping_MethodService.GetShipping_MethodsListAsync();
            var shipping_MethodsListMapper = _mapper.Map<List<GetShipping_MethodListResponse>>(shipping_MethodsList);

            var result = Success(shipping_MethodsListMapper);
            result.Meta = new { Count = shipping_MethodsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleShipping_MethodResponse>> Handle(GetShipping_MethodByIdQuery request, CancellationToken cancellationToken)
        {
            var shipping_Method = await _shipping_MethodService.GetShipping_MethodByIdAsync(request.Id);

            if (shipping_Method == null) return NotFound<GetSingleShipping_MethodResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleShipping_MethodResponse>(shipping_Method);
            return Success(result);
        }
        #endregion
    }
}
