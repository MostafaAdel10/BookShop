using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.CartItem.Queries.Models;
using BookShop.Core.Features.CartItem.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Queries.Handlers
{
    public class CartItemQueryHandler : ResponseHandler,
            IRequestHandler<GetCartItemsByCurrentUserIdQuery, Response<List<GetCartItemsByCurrentUserIdResponse>>>
    {
        #region Fields
        private readonly ICartItemService _cartItemService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CartItemQueryHandler(ICartItemService cartItemService, IMapper mapper, ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cartItemService = cartItemService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetCartItemsByCurrentUserIdResponse>>> Handle(GetCartItemsByCurrentUserIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            var cartItems = await _cartItemService.GetCartItemsByUserIdAsync(userId);
            if (cartItems == null || !cartItems.Any())
                return NotFound<List<GetCartItemsByCurrentUserIdResponse>>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var mappedCartItems = cartItems.Select(ci => new GetCartItemsByCurrentUserIdResponse
            {
                Id = ci.Id,
                BookId = ci.BookId,
                BookName = ci.Book.Title,
                OriginalPrice = ci.Book.Price,
                DiscountedPrice = ci.Book.PriceAfterDiscount,
                Quantity = ci.Quantity,
                ShoppingCartId = ci.ShoppingCartId
            }).ToList();

            var result = Success(mappedCartItems);
            result.Meta = new { Count = mappedCartItems.Count };
            return result;
        }
        #endregion
    }
}