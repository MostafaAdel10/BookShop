using BookShop.Core.Features.Payment_Methods.Commands.Models;

namespace BookShop.Core.Mapping.Payment_Methods
{
    public partial class Payment_MethodsProfile
    {
        public void EditPayment_MethodsCommandMapping()
        {
            CreateMap<EditPayment_MethodsCommand, DataAccess.Entities.Payment_Methods>();
        }
    }
}
