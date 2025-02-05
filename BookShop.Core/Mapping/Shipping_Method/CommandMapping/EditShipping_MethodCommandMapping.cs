using BookShop.Core.Features.Shipping_Method.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Shipping_Method
{
    public partial class Shipping_MethodProfile
    {
        public void EditShipping_MethodCommandMapping()
        {
            CreateMap<EditShipping_MethodCommand, Shipping_Methods>();
        }
    }
}
