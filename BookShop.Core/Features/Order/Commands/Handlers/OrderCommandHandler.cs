using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Features.OrderItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Handlers
{
    public class OrderCommandHandler : ResponseHandler,
            IRequestHandler<AddOrderCommandAPI, Response<OrderCommand>>,
            IRequestHandler<EditOrderCommand, Response<OrderCommand>>,
            IRequestHandler<CancelOrderCommand, Response<string>>,
            IRequestHandler<UpdateOrderStateCommand, Response<string>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IPayment_MethodsService _paymentMethodService;
        private readonly IShipping_MethodService _shippingMethodService;
        private readonly IOrder_StateService _orderStateService;
        private readonly IBook_DiscountService _book_DiscountService;
        private readonly IBookService _bookService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private string key = "Orders";
        #endregion

        #region Constructors
        public OrderCommandHandler(IOrderService orderService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer,
            IPayment_MethodsService paymentMethodService,
            IShipping_MethodService shippingMethodService, IOrder_StateService orderStateService,
            IBook_DiscountService book_DiscountService, IBookService bookService,
            IOrderItemService orderItemService, IApplicationUserService applicationUserService) : base(stringLocalizer)
        {
            _orderService = orderService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _paymentMethodService = paymentMethodService;
            _shippingMethodService = shippingMethodService;
            _orderStateService = orderStateService;
            _book_DiscountService = book_DiscountService;
            _bookService = bookService;
            _orderItemService = orderItemService;
            _applicationUserService = applicationUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<OrderCommand>> Handle(AddOrderCommandAPI request, CancellationToken cancellationToken)
        {
            try
            {
                //Create OrderItems
                List<AddOrderItemCommand> items = new();
                foreach (var book in request.Books)
                {
                    items.Add(new AddOrderItemCommand
                    {
                        BookId = book.BookId,
                        Quantity = book.Quantity,
                        Price = book.Price,
                    });

                    await _bookService.EditUnit_InstockOfBookCommand(book.BookId, book.Quantity);

                    var boo = await _bookService.GetByIdAsync(book.BookId);
                    _bookService.RemoveFromCashMemoery("Books", boo);
                    _bookService.AddtoCashMemoery("Books", new List<Book> { boo });
                }
                //Create Order
                var createOrderDTO = new AddOrderCommand
                {
                    UserId = request.UserId,
                    OrderDate = DateTime.Now,
                    ShippingMethodId = request.ShippingMethodId,
                    ShippingAddress = request.Address,
                    TotalAmount = request.TotalAmount,
                    TrackingNumber = request.PhoneNumber,
                    OrderItems = items,
                    OrderStateId = 1,
                    PaymentMethodId = 2
                };

                //// Validate related entities
                var paymentMethod = await _paymentMethodService.GetPayment_MethodByIdAsync(createOrderDTO.PaymentMethodId);
                var shippingMethod = await _shippingMethodService.GetShipping_MethodByIdAsync(createOrderDTO.ShippingMethodId);
                var orderState = await _orderStateService.GetOrder_StateById(createOrderDTO.OrderStateId);

                // Map the Order DTO to Order entity
                var order = new DataAccess.Entities.Order
                {
                    Total_amout = createOrderDTO.TotalAmount,
                    tracking_number = createOrderDTO.TrackingNumber,
                    shipping_address = createOrderDTO.ShippingAddress,
                    OrderDate = createOrderDTO.OrderDate,
                    ShippingMethodsID = createOrderDTO.ShippingMethodId,
                    OrderStateID = createOrderDTO.OrderStateId,
                    ApplicationUserId = createOrderDTO.UserId,
                    PaymentMethodsID = createOrderDTO.PaymentMethodId,
                    Code = (await _orderService.MaxCode()) + 1
                };
                order.payment_Methods = paymentMethod;
                order.shipping_Methods = shippingMethod;
                order.order_State = orderState;

                //Add order
                var createdOrder = await _orderService.AddAsyncReturnId(order);


                //===============================================================            
                // Create Order Items and link to the order
                foreach (var item in createOrderDTO.OrderItems)
                {
                    var orderItem = new DataAccess.Entities.OrderItem
                    {
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Tax = item.Tax,
                        OrderId = createdOrder.Id,
                        Orders = createdOrder,
                        BookId = item.BookId,

                    };
                    await _orderItemService.AddAsync(orderItem);
                }

                //===============================================================
                // Map to OrderDTO for returning
                createdOrder.order_State = (await _orderStateService.GetOrder_StateById(createOrderDTO.OrderStateId));
                createdOrder.shipping_Methods = await _shippingMethodService.GetShipping_MethodByIdAsync(createOrderDTO.ShippingMethodId);
                createdOrder.payment_Methods = await _paymentMethodService.GetPayment_MethodByIdAsync(createOrderDTO.PaymentMethodId);
                createdOrder.ApplicationUser = await _applicationUserService.GetByIdAsync(createOrderDTO.UserId);

                var returnOrder = new OrderCommand(createdOrder);

                if (returnOrder != null)
                {
                    _orderService.AddtoCashMemoery(key, new List<DataAccess.Entities.Order> { createdOrder });

                    return Created(returnOrder);
                }
                else
                {
                    return BadRequest<OrderCommand>();
                }
            }
            catch (Exception ex)
            {
                return BadRequest<OrderCommand>(ex.Message);
            }
        }

        public async Task<Response<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderService.GetByIdAsyncWithInclude(request.OrderId);
                //Return NotFound
                if (order == null) return NotFound<string>();

                if (order.OrderItems != null)
                {
                    foreach (var item in order.OrderItems)
                    {
                        await _bookService.EditUnit_InstockOfBookCommand(item.BookId, item.Quantity, isSubtract: false);

                        var boo = await _bookService.GetByIdAsync(item.BookId);
                        _bookService.RemoveFromCashMemoery("Books", boo);
                        _bookService.AddtoCashMemoery("Books", new List<Book> { boo });
                    }
                }

                var result = await _orderService.DeleteOrderAndOrderItemsAsync(request.OrderId);

                _orderService.RemoveFromCashMemoery(key, order);

                if (result == true)
                    return Deleted<string>();
                else
                    return BadRequest<string>();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<string>(ex.Message);
            }
        }

        public async Task<Response<string>> Handle(UpdateOrderStateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(request.OrderId);
                //Return NotFound
                if (order == null) return NotFound<string>();

                order.OrderStateID = request.OrderStateId;
                await _orderService.EditAsync(order);


                var orderCash = await _orderService.GetByIdAsync(request.OrderId);
                _orderService.RemoveFromCashMemoery(key, orderCash);
                _orderService.AddtoCashMemoery(key, new List<DataAccess.Entities.Order> { orderCash });

                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<string>(ex.Message);
            }
        }

        public async Task<Response<OrderCommand>> Handle(EditOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderService.GetByIdAsyncWithInclude(request.Id);
                //Return NotFound
                if (order == null) return NotFound<OrderCommand>();

                //mapping
                order.Code = request.Code;
                order.tracking_number = request.TrackingNumber;
                order.shipping_address = request.ShippingAddress;
                order.ShippingMethodsID = request.ShippingMethodId;
                order.OrderStateID = request.OrderStateId;
                order.Updated_at = DateTime.Now;
                order.PaymentMethodsID = 2;

                // Update related entities
                var paymentMethod = await _paymentMethodService.GetPayment_MethodByIdAsync(order.PaymentMethodsID);
                var shippingMethod = await _shippingMethodService.GetShipping_MethodByIdAsync(order.ShippingMethodsID);
                var orderState = await _orderStateService.GetOrder_StateById(order.OrderStateID);
                order.payment_Methods = paymentMethod;
                order.shipping_Methods = shippingMethod;
                order.order_State = orderState;

                var updatedOrderMapping = new OrderCommand(order);

                await _orderService.EditAsync(order);
                return Success(updatedOrderMapping, _localizer[SharedResourcesKeys.Updated]);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<OrderCommand>(ex.Message);
            }
        }
        #endregion
    }
}
