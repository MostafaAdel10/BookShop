using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.CartItem.Queries.Models;
using BookShop.Core.Features.CartItem.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Queries.Handlers
{
    public class CartItemQueryHandler : ResponseHandler,
            IRequestHandler<GetCartItemListQuery, Response<List<GetCartItemListResponse>>>,
            IRequestHandler<GetCartItemByIdQuery, Response<GetSingleCartItemResponse>>
    {
        #region Fields
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CartItemQueryHandler(ICartItemService cartItemService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cartItemService = cartItemService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetCartItemListResponse>>> Handle(GetCartItemListQuery request, CancellationToken cancellationToken)
        {
            var cartItemsList = await _cartItemService.GetCartItemsListAsync();
            var cartItemsListMapper = _mapper.Map<List<GetCartItemListResponse>>(cartItemsList);

            var result = Success(cartItemsListMapper);
            result.Meta = new { Count = cartItemsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleCartItemResponse>> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            var cartItem = await _cartItemService.GetCartItemByIdAsync(request.Id);

            if (cartItem == null) return NotFound<GetSingleCartItemResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleCartItemResponse>(cartItem);
            return Success(result);
        }
        #endregion
    }
}
