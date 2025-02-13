using BookShop.Core.Bases;
using BookShop.DataAccess.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

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
            ISBN13 = book.ISBN13;
            ISBN10 = book.ISBN10;
            Author = book.Author;
            Price = book.Price;
            PriceAfterDiscount = book.PriceAfterDiscount;
            Publisher = book.Publisher;
            PublicationDate = book.PublicationDate;
            Unit_Instock = book.Unit_Instock;
            IsActive = book.IsActive;
            SubjectId = book.SubjectId;
            SubSubjectId = book.SubSubjectId;
            Image = book.Image_url;
            Discounts = book.Discount != null ? book.Discount.Select(b => b.discount != null ? b.discount.Id : 0).ToList() : new List<int>();
            Updated_By = book.Updated_By;
        }
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

        public IFormFile? ImageD { get; set; }
        public string? Image { get; set; }
        public int? Updated_By { get; set; }

        public int DiscountId { get; set; }
        public ICollection<int>? Discounts { get; set; }
    }
    public record EditUnit_InstockOfBookCommand : IRequest<Response<string>>
    {
        public int BookId { get; set; }
        public int quantity { get; set; }
        public bool IsSubtract { get; set; } = true;
    }
}
