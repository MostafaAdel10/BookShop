using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class Discount : BaseEntity<int>
    {
        //public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } //اسم الخصم مثل "خصم الشتاء.



        [MaxLength(300)]
        public string? ImageUrl { get; set; }


        [Range(200, 1_000)]
        public int? Code { get; set; }


        public DateTime Start_date { get; set; }

        public DateTime End_date { get; set; }

        public bool IsActive { get; set; }


        [Range(0, 100)]
        [Column(TypeName = "money")]
        public decimal Percentage { get; set; }

        // Navigation Property
        public virtual ICollection<Book_Discount>? Book_Discounts { get; set; }
    }
}
