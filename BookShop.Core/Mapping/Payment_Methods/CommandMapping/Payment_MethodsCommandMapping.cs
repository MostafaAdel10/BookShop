using BookShop.Core.Features.Payment_Methods.Commands.Models;

namespace BookShop.Core.Mapping.Payment_Methods
{
    public partial class Payment_MethodsProfile
    {
        public void Payment_MethodsCommandMapping()
        {
            CreateMap<DataAccess.Entities.Payment_Methods, Payment_MethodsCommand>();
        }
    }
}
