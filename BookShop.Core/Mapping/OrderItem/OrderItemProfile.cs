using AutoMapper;

namespace BookShop.Core.Mapping.OrderItem
{
    public partial class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            EditOrderItemCommandMapping();
            OrderItemCommandMapping();
        }
    }
}
