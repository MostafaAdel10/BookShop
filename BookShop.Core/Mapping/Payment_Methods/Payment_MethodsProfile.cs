using AutoMapper;

namespace BookShop.Core.Mapping.Payment_Methods
{
    public partial class Payment_MethodsProfile : Profile
    {
        public Payment_MethodsProfile()
        {
            AddPayment_MethodsCommandMapping();
            Payment_MethodsCommandMapping();
            EditPayment_MethodsCommandMapping();
            GetPayment_MethodsListMapping();
            GetPayment_MethodsByIdMapping();
        }
    }
}
