using BookShop.Core.Wrappers;

namespace BookShop.Core.Features.Subject.Queries.Response_DTO_
{
    public class GetSubjectByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetSubSubjectListResponse>? SubSubjectsList { get; set; }
        public PaginatedResult<GetBooksListResponse>? BooksList { get; set; }
    }
    public class GetSubSubjectListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetBooksListResponse
    {
        public GetBooksListResponse(int id, string title, string author,
            decimal price, decimal? priceAfterDiscount, string publisher, DateTime publicationDate, int unit_Instock,
            string image_url, bool isActive)
        {
            Id = id;
            Title = title;
            Author = author;
            Price = price;
            PriceAfterDiscount = priceAfterDiscount;
            Publisher = publisher;
            PublicationDate = publicationDate;
            Unit_Instock = unit_Instock;
            Image_url = image_url;
            IsActive = isActive;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterDiscount { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Unit_Instock { get; set; }
        public string Image_url { get; set; }
        public bool IsActive { get; set; }
    }
}
