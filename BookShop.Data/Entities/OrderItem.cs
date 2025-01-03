using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int Quantity { get; set; }  // عدد النسخ أو الكمية


        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int Tax { get; set; }


        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }  // معرف الكتاب (أو المنتج) الموجود في السلة


        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        

        // Navigation Properties
        public virtual Order Orders { get; set; }
        public virtual Book? book { get; set; }
    }

}
