using BookShop.DataAccess.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Order
    {

        [Key]
        public int Id { get; set; }


        public DateTime OrderDate { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }

        [Column(TypeName = "money")]
        public decimal Total_amout { get; set; }

        [Required]
        public string tracking_number { get; set; }



        //ForeignKeys
        [ForeignKey(nameof(Shipping_Methods))]
        public int ShippingMethodsID { get; set; }


        [ForeignKey(nameof(Order_State))]
        public int OrderStateID { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }


        // Navigation Properties
        public virtual Payment? Payment { get; set; }
        public virtual Order_State? order_State { get; set; }
        public virtual Shipping_Methods? shipping_Methods { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }  // يمثل المستخدم المرتبط بالطلب
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public virtual Address Address { get; set; } // علاقة One-to-One مع ShippingAddress
    }

}
