using System.ComponentModel.DataAnnotations;

namespace FinancePlus.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
