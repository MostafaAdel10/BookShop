using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Card_Type.Queries.Models;
using BookShop.Core.Features.Card_Type.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Card_Type.Queries.Handlers
{
    public class Cart_TypeQueryHandler : ResponseHandler,
            IRequestHandler<GetCart_TypeListQuery, Response<List<GetCart_TypeListResponse>>>,
            IRequestHandler<GetCart_TypeByIdQuery, Response<GetSingleCart_TypeResponse>>
    {
        #region Fields
        private readonly ICart_TypeService _cart_TypeService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public Cart_TypeQueryHandler(ICart_TypeService cart_TypeService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cart_TypeService = cart_TypeService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetCart_TypeListResponse>>> Handle(GetCart_TypeListQuery request, CancellationToken cancellationToken)
        {
            var cart_TypesList = await _cart_TypeService.GetCart_TypesListAsync();
            var cart_TypesListMapper = _mapper.Map<List<GetCart_TypeListResponse>>(cart_TypesList);

            var result = Success(cart_TypesListMapper);
            result.Meta = new { Count = cart_TypesListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleCart_TypeResponse>> Handle(GetCart_TypeByIdQuery request, CancellationToken cancellationToken)
        {
            var cart_Type = await _cart_TypeService.GetCart_TypeByIdAsync(request.Id);

            if (cart_Type == null) return NotFound<GetSingleCart_TypeResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleCart_TypeResponse>(cart_Type);
            return Success(result);
        }
        #endregion
    }
}
