using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Commands.Validations;
using BookShop.DataAccess.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public class EditBookCommand : IRequest<Response<EditBookCommand>>
    {
        public EditBookCommand()
        {

        }
        public EditBookCommand(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Description = book.Description;
            ISBN13 = book.ISBN13 ?? string.Empty;
            ISBN10 = book.ISBN10 ?? string.Empty;
            Author = book.Author;
            Price = book.Price;
            PriceAfterDiscount = book.PriceAfterDiscount ?? 0;
            Publisher = book.Publisher;
            PublicationDate = book.PublicationDate;
            Unit_Instock = book.Unit_Instock;
            IsActive = book.IsActive;
            SubjectId = book.SubjectId;
            SubSubjectId = book.SubSubjectId;
            Image = book.Image_url;
            Discounts = book.Discount != null ? book.Discount.Select(b => b.discount != null ? b.discount.Id : 0).ToList() : new List<int>();
            Updated_By = book.Updated_By ?? 0;
        }
        public int? Updated_By { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ISBN13 { get; set; }
        public string? ISBN10 { get; set; }
        public string Author { get; set; }
        public decimal? Price { get; set; } // السعر الأساسي للكتاب
        public decimal? PriceAfterDiscount { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? Unit_Instock { get; set; }
        public bool IsActive { get; set; }

        //ForeignKey
        public int SubjectId { get; set; } // Optional Subject
        public int SubSubjectId { get; set; } // Optional SubSubject

        public ICollection<int>? Discounts { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Please enter book Image")]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".svg", ".webp" })]
        public IFormFile? ImageD { get; set; }
    }
}
