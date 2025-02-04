using BookShop.Core.Features.Card_Type.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Cart_Type
{
    public partial class Cart_TypeProfile
    {
        public void EditCart_TypeCommandMapping()
        {
            CreateMap<EditCart_TypeCommand, Card_Type>();
        }
    }
}
