using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookShop.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            Orders = new List<Order>();
            UserReviews = new List<Review>();
        }
        [MaxLength(25)]
        public string? FirstName { get; set; }
        [MaxLength(25)]
        public string? LastName { get; set; }

        [MaxLength(255)]
        public DateTime? BirthDate { get; set; }
        [MaxLength(255)]
        public string? City { get; set; }
        [MaxLength(255)]

        public string? Region { get; set; }
        [MaxLength(25)]

        public string? PostalCode { get; set; }
        [MaxLength(255)]

        public string? Country { get; set; }

        
        [ForeignKey(nameof(Payment_Methods))]
        public int? Payment_MethodsID { get; set; }


        // Navigation Property
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? UserReviews { get; set; }
        public virtual Payment_Methods? payment_Methods { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }

    }
}
