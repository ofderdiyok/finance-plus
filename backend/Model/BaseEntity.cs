using System.ComponentModel.DataAnnotations;

namespace FinancePlus.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public Guid Uuid { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
    }
}
