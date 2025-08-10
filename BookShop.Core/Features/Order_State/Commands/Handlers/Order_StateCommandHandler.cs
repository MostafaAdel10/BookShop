using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Order_State.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order_State.Commands.Handlers
{
    public class Order_StateCommandHandler : ResponseHandler,
            IRequestHandler<AddOrder_StateCommand, Response<Order_StateCommand>>,
            IRequestHandler<EditOrder_StateCommand, Response<Order_StateCommand>>,
            IRequestHandler<DeleteOrder_StateCommand, Response<string>>
    {
        #region Fields
        private readonly IOrder_StateService _order_StateService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Order_StateCommandHandler(IOrder_StateService order_StateService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _order_StateService = order_StateService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<Order_StateCommand>> Handle(AddOrder_StateCommand request, CancellationToken cancellationToken)
        {
            var orderStateEntity = _mapper.Map<DataAccess.Entities.Order_State>(request);

            var result = await _order_StateService.AddAsync(orderStateEntity);

            if (result == "Success")
            {
                // تحويل الكيان إلى DTO وإرجاعه
                var returnOrderState = _mapper.Map<Order_StateCommand>(orderStateEntity);
                return Created(returnOrderState);
            }

            return BadRequest<Order_StateCommand>();
        }

        public async Task<Response<Order_StateCommand>> Handle(EditOrder_StateCommand request, CancellationToken cancellationToken)
        {
            var orderStateEntity = await _order_StateService.GetOrder_StateById(request.Id);
            if (orderStateEntity == null)
                return NotFound<Order_StateCommand>();

            _mapper.Map(request, orderStateEntity);

            var result = await _order_StateService.EditAsync(orderStateEntity);

            if (result == "Success")
            {
                var returnOrderState = _mapper.Map<Order_StateCommand>(orderStateEntity);
                return Success(returnOrderState, _localizer[SharedResourcesKeys.Updated]);
            }

            return BadRequest<Order_StateCommand>();
        }

        public async Task<Response<string>> Handle(DeleteOrder_StateCommand request, CancellationToken cancellationToken)
        {
            var orderStateEntity = await _order_StateService.GetOrder_StateById(request.Id);
            if (orderStateEntity == null)
                return NotFound<string>();

            var result = await _order_StateService.DeleteAsync(orderStateEntity);

            return result == "Success"
                ? Deleted<string>()
                : BadRequest<string>();
        }
        #endregion
    }
}
