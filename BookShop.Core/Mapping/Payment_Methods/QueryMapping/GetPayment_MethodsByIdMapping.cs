using BookShop.Core.Features.Payment_Methods.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.Payment_Methods
{
    public partial class Payment_MethodsProfile
    {
        public void GetPayment_MethodsByIdMapping()
        {
            CreateMap<DataAccess.Entities.Payment_Methods, GetSinglePayment_MethodsResponse>();
        }
    }
}
