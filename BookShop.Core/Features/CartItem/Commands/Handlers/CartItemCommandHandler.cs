using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Commands.Handlers
{
    public class CartItemCommandHandler : ResponseHandler
    {
        #region Fields
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CartItemCommandHandler(ICartItemService cartItemService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cartItemService = cartItemService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
