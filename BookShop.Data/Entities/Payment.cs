using BookShop.DataAccess.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum PaymentMethodType
    {
        CashOnDelivery,
        CreditCard,
        PayPal
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        public string? TransactionId { get; private set; }

        // يمكن تخزين نوع الدفع إذا أردت
        public PaymentMethodType PaymentMethod { get; set; }

        // المفاتيح الخارجية
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public int ApplicationUserId { get; set; }

        // علاقات الملاحة
        public virtual Order Order { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        // طريقة لتحديث الحالة ورقم المعاملة مع التحقق من صحة البيانات
        public void UpdateStatus(PaymentStatus status, string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                throw new ArgumentException("TransactionId cannot be null or empty", nameof(transactionId));
            }
            Status = status;
            TransactionId = transactionId;
        }
    }
}
