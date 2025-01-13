namespace BookShop.Core.Features.Books.Queries.Results
{
    public class GetBookPaginatedListResponse
    {
        public GetBookPaginatedListResponse(int id, string title, string description, string isbn13, string author,
            decimal price, decimal? priceAfterDiscount, string publisher, DateTime publicationDate, int unit_Instock,
            string image_url, bool isActive, string? subjectName, string? subSubjectName)
        {
            Id = id;
            Title = title;
            Description = description;
            ISBN13 = isbn13;
            Author = author;
            Price = price;
            PriceAfterDiscount = priceAfterDiscount;
            Publisher = publisher;
            PublicationDate = publicationDate;
            Unit_Instock = unit_Instock;
            Image_url = image_url;
            IsActive = isActive;
            SubjectName = subjectName;
            SubSubjectName = subSubjectName;
        }
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

        public string SubjectName { get; set; } // Optional Subject

        public string SubSubjectName { get; set; } // Optional SubSubject

    }
}
