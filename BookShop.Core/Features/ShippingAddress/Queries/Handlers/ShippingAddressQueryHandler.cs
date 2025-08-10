using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.ShippingAddress.Queries.Models;
using BookShop.Core.Features.ShippingAddress.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.ShippingAddress.Queries.Handlers
{
    public class ShippingAddressQueryHandler : ResponseHandler,
            IRequestHandler<GetShippingAddressesByCurrentUserIdQuery, Response<List<GetShippingAddressesByCurrentUserIdResponse>>>
    {
        #region Fields
        private readonly IAddressService _addressService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ShippingAddressQueryHandler(IAddressService addressService, IMapper mapper, ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _addressService = addressService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetShippingAddressesByCurrentUserIdResponse>>> Handle(GetShippingAddressesByCurrentUserIdQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserService.GetUserId();

            var addresses = await _addressService.GetAddressesByUserIdAsync(currentUser);
            if (addresses is null || addresses.Count == 0)
                return NotFound<List<GetShippingAddressesByCurrentUserIdResponse>>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var mappedAddresses = _mapper.Map<List<GetShippingAddressesByCurrentUserIdResponse>>(addresses);
            return Success(mappedAddresses);
        }
        #endregion
    }
}
