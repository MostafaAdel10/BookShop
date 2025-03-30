using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Review : BaseEntity<int>
    {
        [Range(1, 5)]
        public int Rating { get; set; } // تصنيف من 1 إلى 5

        [Required]
        public string Content { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        // Navigation Properties
        public Book? Book { get; set; }
        public virtual ICollection<User_Reviews>? UserReviews { get; set; }
    }

}
