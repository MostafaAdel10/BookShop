using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Commands.Validations;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace BookShop.Core.Features.Books.Commands.Models
{
    public class AddBookCommand : IRequest<Response<AddBookCommand>>
    {
        public int? CreatedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ISBN13 { get; set; }
        public string? ISBN10 { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; } // السعر الأساسي للكتاب
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Unit_Instock { get; set; }
        public string? Image_url { get; set; }
        public bool IsActive { get; set; }

        //ForeignKey
        public int SubjectId { get; set; } // Optional Subject
        public int SubSubjectId { get; set; } // Optional SubSubject

        public ICollection<int>? Discounts { get; set; }

        [Required(ErrorMessage = "Please enter book Image")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".svg", ".webp" })]
        public IFormFile? ImageData { get; set; }
    }
}
