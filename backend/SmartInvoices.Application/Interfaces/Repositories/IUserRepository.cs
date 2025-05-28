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
}
