using Microsoft.EntityFrameworkCore;
using FinancePlus.Models;

namespace FinancePlus.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSet'ler
        public DbSet<User> Users => Set<User>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Transfer> Transfers => Set<Transfer>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Transaction → User
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            // Transaction → Category
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId);

            // Transfer → Sender
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.Sender)
                .WithMany(u => u.TransfersSent)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transfer → Receiver
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.Receiver)
                .WithMany(u => u.TransfersReceived)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
