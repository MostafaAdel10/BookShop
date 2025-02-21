using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Book : BaseEntity<int>
    {
        // Constructor for initializing collections
        public Book()
        {
            Discount = new List<Book_Discount>();
            Images = new List<Book_Image>();
            Reviews = new List<Review>();
            OrderItems = new List<OrderItem>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }


        public string Description { get; set; }


        [MaxLength(13)]
        public string? ISBN13 { get; set; }

        [MaxLength(10)]
        public string? ISBN10 { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // السعر الأساسي للكتاب


        [Column(TypeName = "money")]
        public decimal? PriceAfterDiscount { get; set; }


        [MaxLength(100)]
        public string Publisher { get; set; } = string.Empty;


        public DateTime PublicationDate { get; set; }

        public int Unit_Instock { get; set; }


        [MaxLength(300)]
        public string Image_url { get; set; }

        public bool IsActive { get; set; } = false;



        //ForeignKey

        [ForeignKey("Subject")]
        public int SubjectId { get; set; } // Optional Subject

        [ForeignKey("SubSubject")]
        public int SubSubjectId { get; set; } // Optional SubSubject


        // Navigation Properties
        public Subject? Subject { get; set; }
        public SubSubject? SubSubject { get; set; }


        public virtual ICollection<Book_Image>? Images { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Book_Discount>? Discount { get; set; }

        //public ICollection<Wishlist>? Wishlists { get; set; }
    }
}
