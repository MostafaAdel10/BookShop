using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.ShoppingCart.Queries.Handlers
{
    public class ShoppingCartQueryHandler : ResponseHandler
    {
        #region Fields
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ShoppingCartQueryHandler(IShoppingCartService shoppingCartService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        #endregion
    }
}
