using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

       

        // Navigation Properties
        public ICollection<SubSubject>? SubSubjects { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
