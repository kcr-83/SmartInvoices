namespace SmartInvoices.WebApi.Endpoints.Auth.Login;

/// <summary>
/// Model żądania logowania.
/// </summary>
public class LoginRequest
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
