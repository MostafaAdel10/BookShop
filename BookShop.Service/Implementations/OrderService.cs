using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Enums;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Service.Implementations
{
    public class OrderService : IOrderService
    {
        #region Fields
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMemoryCache _memoryCache;
        private string key = "Orders";
        #endregion

        #region Contractors
        public OrderService(IOrderRepository orderRepository, IMemoryCache memoryCache, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _memoryCache = memoryCache;
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region Handle Functions  
        public async Task<string> AddAsync(Order order)
        {
            //Added Order
            await _orderRepository.AddAsync(order);
            return "Success";
        }
        public async Task<Order> AddAsyncReturnId(Order order)
        {
            //Added Order
            return await _orderRepository.AddAsync(order);
        }
        public async Task<string> DeleteAsync(Order order)
        {
            var transaction = _orderRepository.BeginTransaction();

            try
            {
                await _orderRepository.DeleteAsync(order);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }
        public async Task<string> EditAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
            return "Success";
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order;
        }
        public async Task<Order> GetByIdAsyncWithInclude(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsyncWithInclude(id);
            return order;
        }
        public async Task<Order?> GetByIdWithIncludeAddressAsync(int orderId)
        {
            return await _orderRepository
                .GetTableAsTracking()
                .Include(o => o.Address)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<Order?> GetOrderWithStateAndItemsAsync(int orderId)
        {
            return await _orderRepository
                .GetTableAsTracking()
                .Include(o => o.order_State)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }


        public async Task<bool> IsOrderIdExist(int id)
        {
            //Check if the Order exists or not
            var order = await _orderRepository.GetTableNoTracking().Where(b => b.Id.Equals(id)).FirstOrDefaultAsync();
            if (order == null) return false;
            return true;
        }

        public IQueryable<Order> GetOrderQueryable()
        {
            return _orderRepository.GetTableNoTracking()
                                  .Include(order => order.Address)
                                  .Include(o => o.ApplicationUser)
                                  .Include(o => o.Payment)
                                  .Include(o => o.shipping_Methods)
                                  .Include(o => o.order_State)
                                  .Include(o => o.OrderItems)
                                  .ThenInclude(o => o.book)
                                  .AsQueryable();
        }
        public IQueryable<Order> FilterOrderPaginatedQueryable(OrderOrderingEnum orderingEnum, string search)
        {
            var queryable = _orderRepository.GetTableNoTracking()
                                  .Include(order => order.Address)
                                  .Include(o => o.ApplicationUser)
                                  .Include(o => o.Payment)
                                  .Include(o => o.shipping_Methods)
                                  .Include(o => o.order_State)
                                  .Include(o => o.OrderItems)
                                  .ThenInclude(o => o.book)
                                  .AsQueryable();
            if (search != null)
            {
                queryable = queryable.Where(o => o.tracking_number.Contains(search));
            }

            switch (orderingEnum)
            {
                case OrderOrderingEnum.Id:
                    queryable = queryable.OrderBy(b => b.Id);
                    break;
                case OrderOrderingEnum.OrderDate:
                    queryable = queryable.OrderBy(b => b.OrderDate);
                    break;
                case OrderOrderingEnum.Total_amout:
                    queryable = queryable.OrderBy(b => b.Total_amout);
                    break;
                case OrderOrderingEnum.tracking_number:
                    queryable = queryable.OrderBy(b => b.tracking_number);
                    break;
                default:
                    queryable = queryable.OrderBy(b => b.Id);
                    break;
            }
            return queryable;
        }

        public async Task<List<Order>> GetOrdersListAsync()
        {
            return await _orderRepository.GetOrdersListAsync();
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetByUserId(userId)
                .Include(order => order.Address)
                .Include(order => order.ApplicationUser)
                .Include(order => order.shipping_Methods)
                .Include(o => o.Payment)
                .Include(order => order.order_State)
                .Include(order => order.OrderItems)
                    .ThenInclude(item => item.book)
                .AsNoTracking()
                .ToListAsync();
        }


        //Delete Oredr
        public async Task<bool> DeleteOrderAndOrderItemsAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsyncWithInclude(id);
                if (order == null) return false;

                var orderItems = order.OrderItems != null ? order.OrderItems.ToList() : new List<OrderItem>();

                foreach (var orderItem in orderItems)
                {
                    await _orderItemRepository.DeleteAsync(orderItem);
                }

                await _orderRepository.DeleteAsync(order);
                return true;
            }
            catch
            {
                return false;
            }
        }


        //Cash
        public void AddToCache(string key, object value, TimeSpan? absoluteExpiration = null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public T GetFromCache<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T value);
            return value;
        }

        public void RemoveFromCache(string key)
        {
            _memoryCache.Remove(key);
        }

        public List<Order> AddtoCashMemoery(string key, List<Order> items)
        {
            var DataIncash = GetFromCache<List<Order>>(key);
            if (DataIncash != null && DataIncash.Count > 0)
            {
                foreach (var item in items)
                {
                    if (!DataIncash.Any(existingItem => existingItem.Id == item.Id))
                    {
                        DataIncash.Insert(0, item);
                    }
                }
                AddToCache(key, DataIncash, TimeSpan.FromMinutes(30));
                return DataIncash;
            }
            else
            {
                AddToCache(key, items, TimeSpan.FromMinutes(30));
                return items;
            }
        }

        public void RemoveFromCashMemoery(string key, Order item)
        {
            var DataIncash = GetFromCache<List<Order>>(key);
            if (DataIncash != null && DataIncash.Count > 0)
            {
                if (DataIncash.Where(existingItem => existingItem.Id == item.Id).ToList().Count == 1)
                {
                    var data = DataIncash.FirstOrDefault(p => p.Id == item.Id);
                    if (data != null)
                    {
                        DataIncash.Remove(data);
                        AddToCache(key, DataIncash, TimeSpan.FromMinutes(30));
                    }
                }
            }
        }

        #endregion
    }
}
