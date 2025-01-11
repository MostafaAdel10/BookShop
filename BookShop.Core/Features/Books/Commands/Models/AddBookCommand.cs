using BookShop.Core.Bases;
using MediatR;


namespace BookShop.Core.Features.Books.Commands.Models
{
    public class AddBookCommand : IRequest<Response<string>>
    {
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
        public int SubjectId { get; set; } // Optional Subject
        public int SubSubjectId { get; set; } // Optional SubSubject
    }
}
