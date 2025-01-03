using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class Payment_Methods : BaseEntity<int>
    {
        public string? Card_Number { get; set; }
        public DateOnly? Expiration_Date { get; set; }
        public bool Is_Default { get; set; } = false;
        public string Name { get; set; }


        [ForeignKey("Card_Type")]
        public int? Card_TypeId { get; set; }


        // Navigation Property
        public virtual Card_Type? Card_type { get; set; }
    }
}
