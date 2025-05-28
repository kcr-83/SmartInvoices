using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SmartInvoices.Domain.Interfaces
{
    /// <summary>
    /// Interfejs kontekstu bazy danych dla aplikacji SmartInvoices
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Faktury
        /// </summary>
        DbSet<Invoice> Invoices { get; }

        /// <summary>
        /// Pozycje faktur
        /// </summary>
        DbSet<LineItem> LineItems { get; }

        /// <summary>
        /// Użytkownicy
        /// </summary>
        DbSet<User> Users { get; }

        /// <summary>
        /// Wnioski o zwrot
        /// </summary>
        DbSet<RefundRequest> RefundRequests { get; }
        
        /// <summary>
        /// Wnioski o zmianę pozycji faktury
        /// </summary>
        DbSet<ChangeRequest> ChangeRequests { get; }
        
        /// <summary>
        /// Załączniki dokumentów
        /// </summary>
        DbSet<DocumentAttachment> DocumentAttachments { get; }
        
        /// <summary>
        /// Logi audytowe
        /// </summary>
        DbSet<AuditLog> AuditLogs { get; }
        
        /// <summary>
        /// Powiadomienia
        /// </summary>
        DbSet<Notification> Notifications { get; }
        
        /// <summary>
        /// Ustawienia powiadomień
        /// </summary>
        DbSet<NotificationSettings> NotificationSettings { get; }
        
        /// <summary>
        /// Zapisuje zmiany w bazie danych
        /// </summary>
        /// <param name="cancellationToken">Token anulowania</param>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Zapisuje zmiany w bazie danych
        /// </summary>
        int SaveChanges();
    }
}
