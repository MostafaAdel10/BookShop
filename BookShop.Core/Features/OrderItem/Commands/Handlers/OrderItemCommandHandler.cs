using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.OrderItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.OrderItem.Commands.Handlers
{
    public class OrderItemCommandHandler : ResponseHandler,
        IRequestHandler<AddOrderItemCommandWithOrderId, Response<OrderItemCommand>>,
        IRequestHandler<EditOrderItemCommandWithOrderId, Response<OrderItemCommand>>,
        IRequestHandler<DeleteOrderItemCommandWithOrderId, Response<string>>
    {
        #region Fields
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public OrderItemCommandHandler(IOrderItemService orderItemService, IBookService bookService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, IOrderService orderService) : base(stringLocalizer)
        {
            _orderItemService = orderItemService;
            _bookService = bookService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _orderService = orderService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<OrderItemCommand>> Handle(AddOrderItemCommandWithOrderId request, CancellationToken cancellationToken)
        {
            //Edit Unit_Instock Of Book
            await _bookService.EditUnit_InstockOfBookCommand(request.BookId, request.Quantity);
            //Add To Cash
            var boo = await _bookService.GetByIdAsync(request.BookId);
            _bookService.RemoveFromCashMemoery("Books", boo);
            _bookService.AddtoCashMemoery("Books", new List<Book> { boo });

            //Mapping between request and Cart_Type
            var orderItemMapper = _mapper.Map<DataAccess.Entities.OrderItem>(request);
            //Add
            var result = await _orderItemService.AddAsyncWithReturnId(orderItemMapper);

            if (result != null)
            {
                // Map back to DTO and return
                var returnOrderItem = _mapper.Map<OrderItemCommand>(result);
                returnOrderItem.BookName = boo.Title ?? string.Empty;
                return Created(returnOrderItem);
            }
            else
                return BadRequest<OrderItemCommand>();
        }

        public async Task<Response<OrderItemCommand>> Handle(EditOrderItemCommandWithOrderId request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(request.Id);
            //get book by id
            var book = await _bookService.GetByIdAsync(request.BookId);
            //Return NotFound
            if (orderItem == null) return NotFound<OrderItemCommand>();
            //chick if book id true or not
            if (request.BookId != orderItem.BookId) return NotFound<OrderItemCommand>(_localizer[SharedResourcesKeys.BookIsNotExist]);
            //chick if price is true or not
            if (request.Price != orderItem.Price) return NotFound<OrderItemCommand>(_localizer[SharedResourcesKeys.PriceIsNotTrue]);
            //Edit Unit_Instock Of Book
            if (request.Quantity > orderItem.Quantity)
            {
                var increaseQuantity = (request.Quantity - orderItem.Quantity);
                if (increaseQuantity > book.Unit_Instock) return UnprocessableEntity<OrderItemCommand>(_localizer[SharedResourcesKeys.QuantityIsGreater]); ;
                await _bookService.EditUnit_InstockOfBookCommand(request.BookId, increaseQuantity);
            }
            else if (request.Quantity < orderItem.Quantity)
            {
                var decreaseQuantity = (orderItem.Quantity - request.Quantity);
                if (decreaseQuantity <= 0) return UnprocessableEntity<OrderItemCommand>(_localizer[SharedResourcesKeys.QuantityCannotBe0OrLess]); ;
                await _bookService.EditUnit_InstockOfBookCommand(request.BookId, decreaseQuantity, isSubtract: false);
            }
            else
            {
                return UnprocessableEntity<OrderItemCommand>(_localizer[SharedResourcesKeys.NoChanges]);
            }


            //Mapping between request and Cart_Type
            var orderItemMapper = _mapper.Map(request, orderItem);
            //Add orderItem
            var result = await _orderItemService.EditAsync(orderItemMapper);
            //Add To Cash
            var boo = await _bookService.GetByIdAsync(request.BookId);
            _bookService.RemoveFromCashMemoery("Books", boo);
            _bookService.AddtoCashMemoery("Books", new List<Book> { boo });
            //response
            if (result != null)
            {
                // Map back to DTO and return
                var returnOrderItem = _mapper.Map<OrderItemCommand>(orderItemMapper);
                returnOrderItem.BookName = boo.Title ?? string.Empty;
                return Success(returnOrderItem, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<OrderItemCommand>();
        }

        public async Task<Response<string>> Handle(DeleteOrderItemCommandWithOrderId request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(request.Id);
            //Return NotFound
            if (orderItem == null) return NotFound<string>();
            //Edit Unit_Instock Of Book
            await _bookService.EditUnit_InstockOfBookCommand(orderItem.BookId, orderItem.Quantity, isSubtract: false);
            //Add To Cash
            var boo = await _bookService.GetByIdAsync(orderItem.BookId);
            _bookService.RemoveFromCashMemoery("Books", boo);
            //Call service that make delete
            var result = await _orderItemService.DeleteAsync(orderItem);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
