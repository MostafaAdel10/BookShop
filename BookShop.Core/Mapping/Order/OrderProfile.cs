using AutoMapper;

namespace BookShop.Core.Mapping.Order
{
    public partial class OrderProfile : Profile
    {
        public OrderProfile()
        {
            OrderCommandMapping();
        }
    }
}
