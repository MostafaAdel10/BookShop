using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Order.Queries.Models;
using BookShop.Core.Features.Order.Queries.Response_DTO_;
using BookShop.Core.Resources;
using BookShop.Core.Wrappers;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookShop.Core.Features.Order.Queries.Handlers
{
    public class OrderQueryHandler : ResponseHandler,
        IRequestHandler<GetOrdersByUserIdQuery, Response<List<GetOrdersByUserIdResponse>>>,
        IRequestHandler<GetOrderByIdQuery, Response<GetOrderByIdResponse>>,
        IRequestHandler<GetOrderListQuery, Response<List<GetOrderListResponse>>>,
        IRequestHandler<GetOrderPaginatedListQuery, PaginatedResult<GetOrderPaginatedListResponse>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion


        #region Constructors
        public OrderQueryHandler(IOrderService orderService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _orderService = orderService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion


        #region Handel Functions
        public async Task<Response<List<GetOrdersByUserIdResponse>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await _orderService.GetOrdersByUserIdAsync(request.UserId);
            if (ordersList == null || !ordersList.Any())
            {
                return NotFound<List<GetOrdersByUserIdResponse>>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            }

            //Mapping
            var ordersListResponse = ordersList.Select(order => new GetOrdersByUserIdResponse
            {
                Id = order.Id,
                Code = order.Code ?? 0,
                OrderDate = order.OrderDate,
                TotalAmount = order.Total_amout,
                TrackingNumber = order.tracking_number,
                ShippingAddress = order.shipping_address,
                ShippingMethod = order.shipping_Methods?.Method_Name ?? "N/A",
                ShippingCost = order.shipping_Methods?.Cost,
                UserId = order.ApplicationUserId,
                OrderState = order.order_State?.Name ?? "N/A",
                OrderStateArabic = order.order_State?.Name_Ar ?? "N/A",
                PaymentMethod = order.payment_Methods?.Name ?? string.Empty,
                OrderItems = order.OrderItems?.Select(item => new OrderItemQuery
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = item.OrderId,
                    Tax = item.Tax,
                    BookName = item.book?.Title ?? "N/A"

                })?.ToList() ?? new List<OrderItemQuery>()
            }).ToList();


            var result = Success(ordersListResponse);
            result.Meta = new { Count = ordersListResponse.Count() };
            return result;
        }

        public async Task<Response<List<GetOrderListResponse>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await _orderService.GetOrderQueryable().ToListAsync();
            //Mapping
            var ordersListResponse = ordersList.Select(order => new GetOrderListResponse
            {
                Id = order.Id,
                Code = order.Code ?? 0,
                OrderDate = order.OrderDate,
                TotalAmount = order.Total_amout,
                TrackingNumber = order.tracking_number,
                ShippingAddress = order.shipping_address,
                ShippingMethod = order.shipping_Methods?.Method_Name ?? "N/A",
                ShippingCost = order.shipping_Methods?.Cost,
                UserId = order.ApplicationUserId,
                OrderState = order.order_State?.Name ?? "N/A",
                OrderStateArabic = order.order_State?.Name_Ar ?? "N/A",
                PaymentMethod = order.payment_Methods?.Name ?? string.Empty,
                OrderItems = order.OrderItems?.Select(item => new OrderItemQuery
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = item.OrderId,
                    Tax = item.Tax,
                    BookName = item.book.Title

                }).ToList() ?? new List<OrderItemQuery>()
            }).ToList();

            var result = Success(ordersListResponse);
            result.Meta = new { Count = ordersListResponse.Count() };
            return result;
        }

        public async Task<PaginatedResult<GetOrderPaginatedListResponse>> Handle(GetOrderPaginatedListQuery request, CancellationToken cancellationToken)
        {
            var allOrders = _orderService.GetOrderQueryable();

            //pagination
            Expression<Func<BookShop.DataAccess.Entities.Order, GetOrderPaginatedListResponse>> expression =
                e => new GetOrderPaginatedListResponse
                {
                    Id = e.Id,
                    Code = e.Code ?? 0,
                    OrderDate = e.OrderDate,
                    TotalAmount = e.Total_amout,
                    TrackingNumber = e.tracking_number,
                    ShippingAddress = e.shipping_address,
                    OrderItems = e.OrderItems.Select(oi => new OrderItemQuery
                    {
                        // مثال على تحويل الخصائص؛ عدل بحسب خصائص OrderItemQuery و OrderItem
                        Id = oi.Id,
                        BookId = oi.BookId,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        OrderId = oi.OrderId,
                        Tax = oi.Tax,
                        BookName = oi.book.Title
                    }).ToList(),
                    ShippingMethod = e.shipping_Methods.Method_Name ?? "N/A",
                    PaymentMethod = e.payment_Methods.Name ?? string.Empty,
                    UserId = e.ApplicationUserId,
                    UserName = e.ApplicationUser.UserName,
                    OrderState = e.order_State.Name ?? "N/A",
                    OrderStateArabic = e.order_State!.Name_Ar ?? "N/A",
                    ShippingCost = e.shipping_Methods.Cost
                };
            var filterQuery = _orderService.FilterOrderPaginatedQueryable(request.OrderBy, request.Search);
            var paginatedList = await filterQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }

        public async Task<Response<GetOrderByIdResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsyncWithInclude(request.Id);

            if (order == null) return NotFound<GetOrderByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            //Mapping
            var ordersListResponse = new GetOrderByIdResponse
            {
                Id = order.Id,
                Code = order.Code ?? 0,
                OrderDate = order.OrderDate,
                TotalAmount = order.Total_amout,
                TrackingNumber = order.tracking_number,
                ShippingAddress = order.shipping_address,
                ShippingMethod = order.shipping_Methods?.Method_Name ?? "N/A",
                ShippingCost = order.shipping_Methods?.Cost,
                UserId = order.ApplicationUserId,
                OrderState = order.order_State?.Name ?? "N/A",
                OrderStateArabic = order.order_State?.Name_Ar ?? "N/A",
                PaymentMethod = order.payment_Methods?.Name ?? string.Empty,
                OrderItems = order.OrderItems?.Select(item => new OrderItemQuery
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    OrderId = item.OrderId,
                    Tax = item.Tax,
                    BookName = item.book.Title

                }).ToList() ?? new List<OrderItemQuery>()
            };

            var result = Success(ordersListResponse);
            return result;
        }


        #endregion
    }
}
