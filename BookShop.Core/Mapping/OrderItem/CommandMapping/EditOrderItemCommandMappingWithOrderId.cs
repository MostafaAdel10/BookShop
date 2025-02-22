using BookShop.Core.Features.OrderItem.Commands.Models;

namespace BookShop.Core.Mapping.OrderItem
{
    public partial class OrderItemProfile
    {
        public void EditOrderItemCommandMappingWithOrderId()
        {
            CreateMap<EditOrderItemCommandWithOrderId, DataAccess.Entities.OrderItem>();
        }
    }
}
