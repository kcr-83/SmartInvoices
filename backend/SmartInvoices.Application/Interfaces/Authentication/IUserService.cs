using SmartInvoices.Application.DTOs.Auth;

namespace SmartInvoices.Application.Interfaces.Authentication;

/// <summary>
/// Interfejs serwisu użytkowników.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Uwierzytelnia użytkownika na podstawie adresu email i hasła.
    /// </summary>
    /// <param name="email">Adres email użytkownika</param>
    /// <param name="password">Hasło użytkownika</param>
    /// <returns>Obiekt użytkownika lub null, jeśli uwierzytelnienie nie powiodło się</returns>
    Task<IUserDto?> AuthenticateAsync(string email, string password);

    /// <summary>
    /// Pobiera użytkownika na podstawie identyfikatora.
    /// </summary>
    /// <param name="userId">Identyfikator użytkownika</param>
    /// <returns>Obiekt użytkownika lub null, jeśli nie znaleziono</returns>
    Task<IUserDto?> GetByIdAsync(int userId);
}
