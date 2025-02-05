using BookShop.Core.Features.Shipping_Method.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Shipping_Method
{
    public partial class Shipping_MethodProfile
    {
        public void Shipping_MethodCommandMapping()
        {
            CreateMap<Shipping_Methods, Shipping_MethodCommand>();
        }
    }
}
