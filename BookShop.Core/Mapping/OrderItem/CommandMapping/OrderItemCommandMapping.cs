using BookShop.Core.Features.OrderItem.Commands.Models;

namespace BookShop.Core.Mapping.OrderItem
{
    public partial class OrderItemProfile
    {
        public void OrderItemCommandMapping()
        {
            CreateMap<DataAccess.Entities.OrderItem, OrderItemCommand>();
        }
    }
}
