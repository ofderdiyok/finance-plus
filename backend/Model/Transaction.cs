using System.ComponentModel.DataAnnotations;

namespace FinancePlus.Models
{
    public class Transaction : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        public string? Description { get; set; }

        public bool IsRecurring { get; set; } = false;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
