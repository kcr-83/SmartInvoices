namespace SmartInvoices.WebApi.Endpoints.Auth.Login;

/// <summary>
/// Model odpowiedzi logowania.
/// </summary>
public class LoginResponse
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
    public UserResponse User { get; set; } = null!;
}

/// <summary>
/// Model użytkownika w odpowiedzi.
/// </summary>
public class UserResponse
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
