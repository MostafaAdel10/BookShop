using System.ComponentModel.DataAnnotations;

namespace BookShop.DataAccess.Entities
{
    public class Card_Type
    {
        [Key]
        public int Id { get; set; }


        [MaxLength(100)]
        public string? Name { get; set; }

        // Navigation Property (One-to-Many Relationship)
        public virtual ICollection<Payment_Methods> Payment_Methods { get; set; } = new List<Payment_Methods>();
    }
}
