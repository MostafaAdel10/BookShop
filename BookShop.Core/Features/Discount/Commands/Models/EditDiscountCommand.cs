using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Commands.Validations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Core.Features.Discount.Commands.Models
{
    public class EditDiscountCommand : IRequest<Response<DiscountCommand>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public int? Code { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public bool IsActive { get; set; }
        public decimal Percentage { get; set; }

        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".svg" })]
        public IFormFile? ImageData { get; set; }
    }
}
