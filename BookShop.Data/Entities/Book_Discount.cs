using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Entities
{
    public class Book_Discount
    {
        public int Id { get; set; }


        [ForeignKey(nameof(Discount))]
        public int DiscountId { get; set; }


        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }


        // Navigation Property
        public virtual Discount? discount { get; set; }
        public virtual Book? book { get; set; }

    }

}
