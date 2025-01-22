using BookShop.DataAccess.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class SubSubject : GeneralLocalizableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_Ar { get; set; }


        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        // Navigation Properties
        public Subject? Subject { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
