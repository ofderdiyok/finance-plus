using System.ComponentModel.DataAnnotations;

namespace FinancePlus.Models
{
    public class Transfer : BaseEntity
    {
        [Required]
        public int SenderId { get; set; }

        public User? Sender { get; set; }

        public int? ReceiverId { get; set; }

        public User? Receiver { get; set; }

        public string? ReceiverIBAN { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string? ReferenceCode { get; set; }

        public TransferType Type { get; set; }

        public bool Verified { get; set; } = false;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

    public enum TransferType
    {
        Internal,
        EFT,
        FAST
    }
}
