using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinancePlus.Models
{
    public class Transaction : BaseEntity
    {
        [JsonIgnore]
        public Guid UserUuid { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Guid CategoryUuid { get; set; }

        public string? Description { get; set; }
    }
}
