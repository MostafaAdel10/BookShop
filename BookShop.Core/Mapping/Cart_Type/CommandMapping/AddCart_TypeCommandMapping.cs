using BookShop.Core.Features.Card_Type.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Cart_Type
{
    public partial class Cart_TypeProfile
    {
        public void AddCart_TypeCommandMapping()
        {
            CreateMap<AddCart_TypeCommand, Card_Type>();
        }
    }
}
