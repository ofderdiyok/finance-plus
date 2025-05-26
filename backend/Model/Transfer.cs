using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinancePlus.Models
{
    public class Transfer : BaseEntity
    {
        [JsonIgnore]
        public Guid senderUuid { get; set; }

        [Required]
        public Guid ReceiverUuid { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]
        public TransferType TransferType { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

    public enum TransferType
    {
        Internal,
        IBAN
    }
}