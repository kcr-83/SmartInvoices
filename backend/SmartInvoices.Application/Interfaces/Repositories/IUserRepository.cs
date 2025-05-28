using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Interfaces.Repositories;

/// <summary>
/// Interfejs repozytorium użytkowników.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Pobiera użytkownika po identyfikatorze.
    /// </summary>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Użytkownik lub null, jeśli nie znaleziono</returns>
    Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Pobiera użytkownika po adresie email.
    /// </summary>
    /// <param name="email">Adres email</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Użytkownik lub null, jeśli nie znaleziono</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera wszystkich użytkowników
    /// </summary>
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Dodaje nowego użytkownika
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aktualizuje istniejącego użytkownika
    /// </summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Usuwa użytkownika
    /// </summary>
    Task DeleteAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera ustawienia powiadomień dla użytkownika
    /// </summary>
    Task<NotificationSettings> GetNotificationSettingsAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Zapisuje ustawienia powiadomień użytkownika
    /// </summary>
    Task SaveNotificationSettingsAsync(NotificationSettings settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera powiadomienia dla użytkownika
    /// </summary>
    Task<IEnumerable<Notification>> GetNotificationsAsync(int userId, bool includeRead = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dodaje nowe powiadomienie
    /// </summary>
    Task AddNotificationAsync(Notification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aktualizuje powiadomienie
    /// </summary>
    Task UpdateNotificationAsync(Notification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Oznacza wszystkie powiadomienia użytkownika jako przeczytane
    /// </summary>
    Task MarkAllNotificationsAsReadAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dodaje wpis do logu audytowego
    /// </summary>
    Task AddAuditLogAsync(AuditLog log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera wpisy z logu audytowego dla danego użytkownika
    /// </summary>
    Task<IEnumerable<AuditLog>> GetAuditLogsByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera wpisy z logu audytowego dla danej encji
    /// </summary>
    Task<IEnumerable<AuditLog>> GetAuditLogsByEntityAsync(string entityType, int entityId, CancellationToken cancellationToken = default);

}
