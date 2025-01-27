using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Queries.Models;
using BookShop.Core.Features.Discount.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Discount.Queries.Handlers
{
    public class DiscountQueryHandler : ResponseHandler,
        IRequestHandler<GetDiscountListQuery, Response<List<GetDiscountListResponse>>>,
        IRequestHandler<GetDiscountByIdQuery, Response<GetSingleDiscountResponse>>
    {
        #region Fields
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public DiscountQueryHandler(IDiscountService discountService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _discountService = discountService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetDiscountListResponse>>> Handle(GetDiscountListQuery request, CancellationToken cancellationToken)
        {
            var discountsList = await _discountService.GetDiscountsListAsync();
            var discountsListMapper = _mapper.Map<List<GetDiscountListResponse>>(discountsList);

            var result = Success(discountsListMapper);
            result.Meta = new { Count = discountsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleDiscountResponse>> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
        {
            var discount = await _discountService.GetDiscountByIdAsync(request.Id);

            if (discount == null) return NotFound<GetSingleDiscountResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleDiscountResponse>(discount);
            return Success(result);
        }
        #endregion
    }
}
