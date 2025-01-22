using BookShop.DataAccess.Commons;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataAccess.Entities
{
    public class Subject : GeneralLocalizableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name_Ar { get; set; }


        // Navigation Properties
        public ICollection<SubSubject>? SubSubjects { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
