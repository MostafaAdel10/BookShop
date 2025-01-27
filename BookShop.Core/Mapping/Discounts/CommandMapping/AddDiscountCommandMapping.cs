using BookShop.Core.Features.Discount.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Discounts
{
    public partial class DiscountProfile
    {
        public void AddDiscountCommandMapping()
        {
            CreateMap<AddDiscountCommand, Discount>();
        }
    }
}
