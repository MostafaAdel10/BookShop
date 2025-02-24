using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            Orders = new List<Order>();
            UserReviews = new List<Review>();
            UserRefreshTokens = new HashSet<UserRefreshToken>();
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

        [EncryptColumn]
        public string? Code { get; set; }


        // Navigation Property
        [InverseProperty(nameof(UserRefreshToken.user))]
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? UserReviews { get; set; }
        public virtual Payment_Methods? payment_Methods { get; set; }
    }
}
