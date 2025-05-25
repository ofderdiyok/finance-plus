using System.ComponentModel.DataAnnotations;

namespace FinancePlus.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        public string? IBAN { get; set; }

        public string? Currency { get; set; } = "TRY";

        public UserType UserType { get; set; } = UserType.Regular;

        public ICollection<Transaction>? Transactions { get; set; }

        public ICollection<Transfer>? TransfersSent { get; set; }

        public ICollection<Transfer>? TransfersReceived { get; set; }
    }

    public enum UserType
    {
        Admin,
        Regular,
        External
    }
}
