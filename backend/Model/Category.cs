using System.ComponentModel.DataAnnotations;

namespace FinancePlus.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        public int? UserId { get; set; } // NULL ise global kategori

        public User? User { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
