namespace FinancePlus.Models
{
    public class AuditLog : BaseEntity
    {
        public string? TableName { get; set; }

        public string? Operation { get; set; } // INSERT, UPDATE, DELETE

        public string? EntityId { get; set; } // UUID string olarak tutulabilir

        public string? PerformedBy { get; set; } // User email ya da ID

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public DateTime ActionTime { get; set; } = DateTime.UtcNow;
    }
}
