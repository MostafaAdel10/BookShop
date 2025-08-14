using BookShop.DataAccess.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public enum PaymentMethodType
    {
        VodafoneCash,
        EtisalatCash
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;

        [Required]
        [MaxLength(10)]

        public string Currency { get; private set; } = "EGP";

        [MaxLength(100)]
        public string? TransactionId { get; private set; }

        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public int ApplicationUserId { get; set; }

        public virtual Order Order { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public void UpdateStatus(PaymentStatus status, string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("TransactionId cannot be null or empty", nameof(transactionId));
            Status = status;
            TransactionId = transactionId;
        }
    }
}
