

namespace BookShop.Core.Features.Books.Queries.Results
{
    public class GetSingleBookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN13 { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; } // السعر الأساسي للكتاب
        public decimal? PriceAfterDiscount { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Unit_Instock { get; set; }
        public string Image_url { get; set; }
        public bool IsActive { get; set; }

        //ForeignKey

        public string? SubjectName { get; set; } // Optional Subject

        public string? SubSubjectName { get; set; } // Optional SubSubject

    }
}
