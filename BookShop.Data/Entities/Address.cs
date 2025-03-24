using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.DataAccess.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }  // اسم المستلم

        [Required]
        [MaxLength(150)]
        public string AddressLine1 { get; set; }  // العنوان الأساسي

        [MaxLength(150)]
        public string? AddressLine2 { get; set; }  // عنوان إضافي (اختياري)

        [Required]
        [MaxLength(50)]
        public string City { get; set; }  // المدينة

        [Required]
        [MaxLength(50)]
        public string State { get; set; }  // المحافظة/الولاية

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; }  // الرمز البريدي

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }  // الدولة

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }  // رقم الهاتف

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }  // مفتاح أجنبي لربط العنوان بالطلب

        // Navigation Property
        public virtual Order Order { get; set; }
    }
}
