using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class Order : BaseEntity<int>
    {

        [Range(1000, int.MaxValue)]
        public int? Code { get; set; }


        public DateTime OrderDate { get; set; }


        [Column(TypeName = "money")]
        public decimal Total_amout { get; set; }


        [MaxLength(15)]
        public string? tracking_number { get; set; }


        [MaxLength(1500)]
        public string? shipping_address { get; set; }



        //ForeignKeys
        [ForeignKey(nameof(Shipping_Methods))]
        public int ShippingMethodsID { get; set; }


        [ForeignKey(nameof(Payment_Methods))]
        public int PaymentMethodsID { get; set; }

        [ForeignKey(nameof(Order_State))]
        public int OrderStateID { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }


        // Navigation Properties
        public virtual Payment_Methods? payment_Methods { get; set; }
        public virtual Order_State? order_State { get; set; }
        public virtual Shipping_Methods? shipping_Methods { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }  // يمثل المستخدم المرتبط بالطلب
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    }

}
