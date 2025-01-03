using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; } // تصنيف من 1 إلى 5

        // Navigation Properties
        public Book? Book { get; set; }
    }

}
