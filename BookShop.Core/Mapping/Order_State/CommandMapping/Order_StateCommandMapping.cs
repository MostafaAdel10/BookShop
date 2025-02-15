using BookShop.Core.Features.Order_State.Commands.Models;

namespace BookShop.Core.Mapping.Order_State
{
    public partial class Order_StateProfile
    {
        public void Order_StateCommandMapping()
        {
            CreateMap<DataAccess.Entities.Order_State, Order_StateCommand>();
        }
    }
}
