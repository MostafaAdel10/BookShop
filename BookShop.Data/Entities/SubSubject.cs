using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class SubSubject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;


        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        // Navigation Properties
        public Subject? Subject { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
