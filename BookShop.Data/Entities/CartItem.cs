using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class CartItem
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }  // معرف الكتاب (أو المنتج) الموجود في السلة

        [Required]
        public int Quantity { get; set; }  // عدد النسخ أو الكمية


        [Required]
        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }  // معرف السلة التي يوجد بها المنتج


        // Navigation Properties
        public virtual ShoppingCart? ShoppingCart { get; set; }
        public virtual Book? Book { get; set; }
    }


}
