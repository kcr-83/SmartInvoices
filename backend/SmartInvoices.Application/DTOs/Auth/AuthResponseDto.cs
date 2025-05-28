namespace SmartInvoices.Application.DTOs.Auth;

/// <summary>
/// DTO dla odpowiedzi autentykacyjnej zawierającej token JWT.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Token JWT do uwierzytelniania.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Data wygaśnięcia tokenu.
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// Informacje o zalogowanym użytkowniku.
    /// </summary>
    public UserDto User { get; set; } = null!;
}

/// <summary>
/// DTO dla informacji o użytkowniku.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Adres email użytkownika.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Imię użytkownika.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Nazwisko użytkownika.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Czy użytkownik ma uprawnienia administratora.
    /// </summary>
    public bool IsAdmin { get; set; }
}
