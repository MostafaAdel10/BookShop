using BookShop.DataAccess.Entities;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public class BookCommand
    {
        public BookCommand()
        {

        }

        public BookCommand(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Description = book.Description;
            ISBN13 = book.ISBN13 ?? string.Empty;
            ISBN10 = book.ISBN10 ?? string.Empty;
            Author = book.Author;
            Price = book.Price;
            PriceAfterDiscount = book.PriceAfterDiscount;
            Publisher = book.Publisher;
            PublicationDate = book.PublicationDate;
            Unit_Instock = book.Unit_Instock;
            IsActive = book.IsActive;
            SubjectId = book.SubjectId;
            SubSubjectId = book.SubSubjectId;
            Image_Url = book.Image_url;
            CreatedBy = book.CreatedBy ?? 0;
            Updated_By = book.Updated_By ?? 0;
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
        public bool IsActive { get; set; }
        public string Image_Url { get; set; }

        //ForeignKey
        public int SubjectId { get; set; } // Optional Subject
        public int SubSubjectId { get; set; } // Optional SubSubject

        public ICollection<string> Discounts { get; set; }
        public int? CreatedBy { get; set; }
        public int? Updated_By { get; set; }
    }
}

