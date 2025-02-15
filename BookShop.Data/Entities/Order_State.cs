using BookShop.DataAccess.Commons;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataAccess.Entities
{
    public class Order_State : GeneralLocalizableEntity
    {
        [Key]
        public int Id { get; set; }


        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Name_Ar { get; set; }

    }
}
