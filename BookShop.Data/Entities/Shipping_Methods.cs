using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Shipping_Methods : BaseEntity<int>
    {
        [MaxLength(50)]
        public string Method_Name { get; set; }


        [Column(TypeName = "money")]
        public decimal Cost { get; set; }


        public DateTime Estimated_Delivery_Time { get; set; }

    }
}
