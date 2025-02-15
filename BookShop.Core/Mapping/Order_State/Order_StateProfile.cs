using AutoMapper;

namespace BookShop.Core.Mapping.Order_State
{
    public partial class Order_StateProfile : Profile
    {
        public Order_StateProfile()
        {
            AddOrder_StateCommandMapping();
            Order_StateCommandMapping();
            EditOrder_StateCommandMapping();
            GetOrder_StateListMapping();
            GetOrder_StateByIdMapping();
        }
    }
}
