using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Book_Image
    {
        public int Id { get; set; }


        [MaxLength(255)]
        public string Image_url { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        // Navigation Property
        public virtual Book? Books { get; set; }
    }
}
