namespace SmartInvoices.Application.DTOs.Auth;

/// <summary>
/// DTO dla danych logowania.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Adres email użytkownika.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hasło użytkownika.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
