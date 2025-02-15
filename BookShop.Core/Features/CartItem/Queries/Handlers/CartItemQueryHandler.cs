using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Queries.Handlers
{
    public class CartItemQueryHandler : ResponseHandler
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
        #endregion
    }
}
