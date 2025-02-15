using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Order_State.Queries.Models;
using BookShop.Core.Features.Order_State.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order_State.Queries.Handlers
{
    public class Order_StateQueryHandler : ResponseHandler,
        IRequestHandler<GetOrder_StateListQuery, Response<List<GetOrder_StateListResponse>>>,
        IRequestHandler<GetOrder_StateByIdQuery, Response<GetSingleOrder_StateResponse>>
    {
        #region Fields
        private readonly IOrder_StateService _order_StateService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion


        #region Constructors
        public Order_StateQueryHandler(IOrder_StateService order_StateService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _order_StateService = order_StateService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetOrder_StateListResponse>>> Handle(GetOrder_StateListQuery request, CancellationToken cancellationToken)
        {
            var order_StatesList = await _order_StateService.GetOrder_StatesListAsync();
            var order_StatesListMapper = _mapper.Map<List<GetOrder_StateListResponse>>(order_StatesList);

            var result = Success(order_StatesListMapper);
            result.Meta = new { Count = order_StatesListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleOrder_StateResponse>> Handle(GetOrder_StateByIdQuery request, CancellationToken cancellationToken)
        {
            var order_State = await _order_StateService.GetOrder_StateById(request.Id);

            if (order_State == null) return NotFound<GetSingleOrder_StateResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetSingleOrder_StateResponse>(order_State);
            return Success(result);
        }
        #endregion
    }
}
