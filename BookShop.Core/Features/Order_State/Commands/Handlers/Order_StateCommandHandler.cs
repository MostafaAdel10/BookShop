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
            //Mapping between request and Order_State
            var order_StateMapper = _mapper.Map<DataAccess.Entities.Order_State>(request);
            //Add
            var result = await _order_StateService.AddAsync(order_StateMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnOrder_State = _mapper.Map<Order_StateCommand>(order_StateMapper);
                return Created(returnOrder_State);
            }
            else
                return BadRequest<Order_StateCommand>();
        }

        public async Task<Response<Order_StateCommand>> Handle(EditOrder_StateCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var order_State = await _order_StateService.GetOrder_StateById(request.Id);
            //Return NotFound
            if (order_State == null) return NotFound<Order_StateCommand>();
            //Mapping between request and order_State
            var order_StateMapper = _mapper.Map(request, order_State);
            //Call service that make edit
            var result = await _order_StateService.EditAsync(order_StateMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnSubject = _mapper.Map<Order_StateCommand>(order_StateMapper);
                return Success(returnSubject, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<Order_StateCommand>();
        }

        public async Task<Response<string>> Handle(DeleteOrder_StateCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var order_State = await _order_StateService.GetOrder_StateById(request.Id);
            //Return NotFound
            if (order_State == null) return NotFound<string>();
            //Call service that make delete
            var result = await _order_StateService.DeleteAsync(order_State);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
