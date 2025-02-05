using BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Shipping_Method
{
    public partial class Shipping_MethodProfile
    {
        public void GetShipping_MethodByIdMapping()
        {
            CreateMap<Shipping_Methods, GetSingleShipping_MethodResponse>();
        }
    }
}
