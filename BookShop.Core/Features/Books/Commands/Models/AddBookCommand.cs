using BookShop.Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace BookShop.Core.Features.Books.Commands.Models
{
    public class AddBookCommand : IRequest<Response<string>>
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN13 { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public decimal Price { get; set; } // السعر الأساسي للكتاب
        public decimal? PriceAfterDiscount { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public int Unit_Instock { get; set; }
        [Required]
        public string Image_url { get; set; }
        [Required]
        public bool IsActive { get; set; }

        //ForeignKey
        [Required]
        public int SubjectId { get; set; } // Optional Subject
        [Required]
        public int SubSubjectId { get; set; } // Optional SubSubject
    }
}
