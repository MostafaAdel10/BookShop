using BookShop.Core.Bases;
using BookShop.Core.Features.Order.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Data;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Order.Commands.Handlers
{
    public class OrderCommandHandler : ResponseHandler,
            IRequestHandler<AddOrderCommand, Response<int>>,
            IRequestHandler<EditOrderCommand, Response<int>>,
            IRequestHandler<CancelOrderCommand, Response<string>>,
            IRequestHandler<UpdateOrderStateCommand, Response<string>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IShipping_MethodService _shippingMethodService;
        private readonly IBookService _bookService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICartItemService _cartItemService;
        private readonly ApplicationDbContext _applicationDBContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public OrderCommandHandler(IOrderService orderService,
            IStringLocalizer<SharedResources> stringLocalizer,
            IShipping_MethodService shippingMethodService,
            IBookService bookService,
            IOrderItemService orderItemService,
            ICurrentUserService currentUserService,
            ICartItemService cartItemService,
            ApplicationDbContext applicationDBContext) : base(stringLocalizer)
        {
            _orderService = orderService;
            _localizer = stringLocalizer;
            _shippingMethodService = shippingMethodService;
            _bookService = bookService;
            _orderItemService = orderItemService;
            _currentUserService = currentUserService;
            _cartItemService = cartItemService;
            _applicationDBContext = applicationDBContext;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<int>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            // Get Current User
            var currentUserId = _currentUserService.GetUserId();

            // Fetch cart items
            var cartItems = await _cartItemService.GetCartItemsByUserIdAsync(currentUserId);
            if (cartItems == null || !cartItems.Any())
                return UnprocessableEntity<int>(_localizer[SharedResourcesKeys.TheShoppingCartIsEmpty]);

            // Get Shipping Method
            var shippingMethod = await _shippingMethodService.GetShipping_MethodByIdAsync(request.ShippingMethodId);
            if (shippingMethod == null)
                return BadRequest<int>(_localizer[SharedResourcesKeys.BadRequest]);

            // Calculate total amount
            decimal totalAmount = cartItems.Sum(ci => (ci.Book.PriceAfterDiscount * ci.Quantity)) + shippingMethod.Cost;

            using var transaction = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                // **التحقق من توفر الكمية لكل كتاب قبل تقليل المخزون**
                foreach (var cartItem in cartItems)
                {
                    var book = await _bookService.GetByIdAsync(cartItem.BookId);
                    if (book == null || book.Unit_Instock < cartItem.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return UnprocessableEntity<int>(_localizer[SharedResourcesKeys.QuantityIsGreater]);
                    }
                }

                // Create and save Shipping Address
                var shippingAddress = new Address
                {
                    FullName = request.FullName,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    State = request.State,
                    PostalCode = request.PostalCode,
                    Country = request.Country,
                    PhoneNumber = request.PhoneNumber
                };

                // Create Order
                var order = new DataAccess.Entities.Order
                {
                    ApplicationUserId = currentUserId,
                    OrderDate = DateTime.UtcNow,
                    Total_amout = totalAmount,
                    tracking_number = Guid.NewGuid().ToString(),
                    ShippingMethodsID = request.ShippingMethodId,
                    PaymentMethodsID = request.PaymentMethodId,
                    OrderStateID = 1,
                    Address = shippingAddress
                };

                var createdOrder = await _orderService.AddAsyncReturnId(order);

                // Create Order Items
                var orderItems = cartItems.Select(ci => new OrderItem
                {
                    OrderId = createdOrder.Id,
                    BookId = ci.BookId,
                    Quantity = ci.Quantity,
                    Price = ci.Book.PriceAfterDiscount,
                }).ToList();

                await _orderItemService.AddRangeAsync(orderItems);

                // Reduce book stock quantity
                foreach (var item in cartItems)
                {
                    await _bookService.EditUnit_InstockOfBookCommand(item.BookId, item.Quantity);
                }

                // Remove cart items
                await _cartItemService.DeleteCartItemsByUserIdAsync(currentUserId);

                await transaction.CommitAsync();

                return Success(createdOrder.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest<int>(_localizer[SharedResourcesKeys.BadRequest]);
            }
        }

        public async Task<Response<int>> Handle(EditOrderCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            // Fetch order with no tracking for performance optimization
            var order = await _orderService.GetByIdWithIncludeAddressAsync(request.OrderId);
            if (order == null)
                return NotFound<int>(_localizer[SharedResourcesKeys.NotFound]);

            if (order.ApplicationUserId != currentUserId)
                return Unauthorized<int>(_localizer[SharedResourcesKeys.UnAuthorized]);

            // Prevent modification of shipped or canceled orders
            if (order.OrderStateID != 1) // 1 = Pending (قابل للتعديل)
                return UnprocessableEntity<int>(_localizer[SharedResourcesKeys.OrderCannotBeEdited]);

            using var transaction = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                // Get old shipping method (if exists)
                var oldShippingMethod = await _shippingMethodService.GetShipping_MethodByIdAsync(order.ShippingMethodsID);

                // Get new shipping method
                var newShippingMethod = await _shippingMethodService.GetShipping_MethodByIdAsync(request.ShippingMethodId);
                if (newShippingMethod == null)
                    return BadRequest<int>(_localizer[SharedResourcesKeys.BadRequest]);

                // Adjust total amount safely
                order.Total_amout -= oldShippingMethod.Cost; // Remove old cost

                order.Total_amout += newShippingMethod.Cost; // Add new cost

                // Update order details
                order.ShippingMethodsID = request.ShippingMethodId;
                order.PaymentMethodsID = request.PaymentMethodId;


                // Update address
                order.Address.FullName = request.FullName;
                order.Address.AddressLine1 = request.AddressLine1;
                order.Address.AddressLine2 = request.AddressLine2;
                order.Address.City = request.City;
                order.Address.State = request.State;
                order.Address.PostalCode = request.PostalCode;
                order.Address.Country = request.Country;
                order.Address.PhoneNumber = request.PhoneNumber;

                // Save changes
                await _orderService.EditAsync(order);
                await transaction.CommitAsync();

                return Success(order.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest<int>(_localizer[SharedResourcesKeys.BadRequest]);
            }
        }

        public async Task<Response<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderWithStateAndItemsAsync(request.OrderId);
            if (order == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            var currentUserId = _currentUserService.GetUserId();
            if (order.ApplicationUserId != currentUserId)
                return Unauthorized<string>();

            if (order.OrderStateID != 1) // 1 = Pending (قابل للتعديل)
                return BadRequest<string>(_localizer[SharedResourcesKeys.OrderCannotBeCanceled]);

            using var transaction = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in order.OrderItems)
                {
                    await _bookService.EditUnit_InstockOfBookCommand(item.BookId, item.Quantity, false);
                }

                order.OrderStateID = 5; // Canceled
                await _orderService.EditAsync(order);

                await transaction.CommitAsync();
                return Success<string>(_localizer[SharedResourcesKeys.CanceledOrder]);
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
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

                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity<string>(ex.Message);
            }
        }
        //delete
        #endregion
    }
}
