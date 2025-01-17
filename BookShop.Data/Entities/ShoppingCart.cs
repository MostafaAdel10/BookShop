using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }  // للتعرف على المستخدم الذي يملك السلة


        [Required]
        public DateTime CreatedAt { get; set; }  // تاريخ إنشاء السلة

        [Required]
        public bool IsActive { get; set; }  // لتحديد إذا كانت السلة نشطة أم تم إتمام الشراء


        // Navigation Property
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }  // المنتجات الموجودة في السلة
    }

}
