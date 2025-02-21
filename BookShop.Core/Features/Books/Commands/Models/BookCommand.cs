using BookShop.DataAccess.Entities;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public class BookCommand
    {
        public BookCommand()
        {

        }
        public BookCommand(EditBookCommand dTO)
        {
            Id = dTO.Id;
            Title = dTO.Title ?? string.Empty;
            Description = dTO.Description;
            ISBN13 = dTO.ISBN13 ?? string.Empty;
            ISBN10 = dTO.ISBN10 ?? string.Empty;
            Author = dTO.Author ?? string.Empty;
            Price = dTO.Price ?? 0;
            PriceAfterDiscount = dTO.PriceAfterDiscount;
            Publisher = dTO.Publisher ?? string.Empty;
            PublicationDate = dTO.PublicationDate;
            Unit_Instock = dTO.Unit_Instock ?? 0;
            SubjectId = dTO.SubjectId;
            SubSubjectId = dTO.SubSubjectId;
            Image_Url = dTO.Image ?? string.Empty;
        }
        public BookCommand(Book book)
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
            SubjectId = book.SubjectId;
            SubSubjectId = book.SubSubjectId;
            Image_Url = book.Image_url;
            if (book.Discount != null)
                Discounts = book.Discount.Select(b => b.discount != null ? b.discount.Percentage.ToString() : string.Empty).ToList();
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
        public string Image_Url { get; set; }

        //ForeignKey
        public int SubjectId { get; set; } // Optional Subject
        public int SubSubjectId { get; set; } // Optional SubSubject

        public ICollection<string> Discounts { get; set; }
        public ICollection<string> ImageURLs { get; set; }
    }
}

