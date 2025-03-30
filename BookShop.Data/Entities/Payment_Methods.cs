using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Payment_Methods : BaseEntity<int>
    {
        public string? Card_Number { get; set; }
        public DateOnly? Expiration_Date { get; set; }
        public bool Is_Default { get; set; } = false;
        public string Name { get; set; }

        public int? Card_TypeId { get; set; }


        // Navigation Property 
        [ForeignKey("Card_TypeId")]
        public virtual Card_Type? Card_type { get; set; }
    }
}
