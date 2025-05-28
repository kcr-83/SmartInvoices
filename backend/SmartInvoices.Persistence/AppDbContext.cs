using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence
{
    /// <summary>
    /// Implementacja kontekstu bazy danych dla aplikacji SmartInvoices
    /// </summary>
    public class AppDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// Konstruktor dla kontekstu bazy danych
        /// </summary>
        /// <param name="options">Opcje konfiguracji dla DbContext</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        /// <summary>
        /// Faktury
        /// </summary>
        public DbSet<Invoice> Invoices => Set<Invoice>();

        /// <summary>
        /// Pozycje faktur
        /// </summary>
        public DbSet<LineItem> LineItems => Set<LineItem>();

        /// <summary>
        /// Użytkownicy
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Wnioski o zwrot
        /// </summary>
        public DbSet<RefundRequest> RefundRequests => Set<RefundRequest>();

        /// <summary>
        /// Wnioski o zmianę pozycji faktury
        /// </summary>
        public DbSet<ChangeRequest> ChangeRequests => Set<ChangeRequest>();

        /// <summary>
        /// Załączniki dokumentów
        /// </summary>
        public DbSet<DocumentAttachment> DocumentAttachments => Set<DocumentAttachment>();

        /// <summary>
        /// Logi audytowe
        /// </summary>
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        /// <summary>
        /// Powiadomienia
        /// </summary>
        public DbSet<Notification> Notifications => Set<Notification>();

        /// <summary>
        /// Ustawienia powiadomień
        /// </summary>
        public DbSet<NotificationSettings> NotificationSettings => Set<NotificationSettings>();

        /// <summary>
        /// Konfiguracja modelu dla bazy danych
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder dla konfiguracji encji</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji dla encji Invoice
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceId);

                entity
                    .HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
                entity.Property(e => e.Notes).HasMaxLength(500);
            });

            // Konfiguracja relacji dla encji LineItem
            modelBuilder.Entity<LineItem>(entity =>
            {
                entity.HasKey(e => e.LineItemId);

                entity
                    .HasOne<Invoice>()
                    .WithMany(i => i.LineItems)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.TaxRate).HasPrecision(5, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
            });

            // Konfiguracja relacji dla encji User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            // Konfiguracja relacji dla encji RefundRequest
            modelBuilder.Entity<RefundRequest>(entity =>
            {
                entity.HasKey(e => e.RefundRequestId);

                entity
                    .HasOne<Invoice>()
                    .WithMany(i => i.RefundRequests)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Reason).HasMaxLength(500);
                entity.Property(e => e.AdminNotes).HasMaxLength(500);
            });

            // Konfiguracja relacji dla encji ChangeRequest
            modelBuilder.Entity<ChangeRequest>(entity =>
            {
                entity.HasKey(e => e.ChangeRequestId);

                entity
                    .HasOne<Invoice>()
                    .WithMany()
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Ignore(e => e.LineItemIds); // Lista identyfikatorów zostanie obsłużona w kodzie aplikacji
            });

            // Konfiguracja relacji dla encji DocumentAttachment
            modelBuilder.Entity<DocumentAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity
                    .HasOne<RefundRequest>()
                    .WithMany()
                    .HasForeignKey(e => e.RefundRequestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FileType).HasMaxLength(100);
            });

            // Konfiguracja relacji dla encji AuditLog
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ActionType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EntityType).IsRequired().HasMaxLength(50);
            });

            // Konfiguracja relacji dla encji Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
            });

            // Konfiguracja relacji dla encji NotificationSettings
            modelBuilder.Entity<NotificationSettings>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity
                    .HasOne<User>()
                    .WithOne()
                    .HasForeignKey<NotificationSettings>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
