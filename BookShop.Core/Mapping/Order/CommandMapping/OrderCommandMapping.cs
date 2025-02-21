using BookShop.Core.Features.Order.Commands.Models;

namespace BookShop.Core.Mapping.Order
{
    public partial class OrderProfile
    {
        public void OrderCommandMapping()
        {
            CreateMap<DataAccess.Entities.Order, OrderCommand>();
        }
    }
}
