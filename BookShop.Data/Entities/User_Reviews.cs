using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class User_Reviews
    {
        public int Id { get; set; }


        [ForeignKey("ApplicationUser")]
        public int ApplicationUser { get; set; }


        [ForeignKey("Reviews")]
        public int ReviewID { get; set; }

        // Navigation Property
        public virtual ApplicationUser applicationUser { get; set; }
        public virtual Review review { get; set; }
    }
}
