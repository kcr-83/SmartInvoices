namespace SmartInvoices.Application.Interfaces.Authentication;

/// <summary>
/// Interfejs DTO dla danych użytkownika.
/// </summary>
public interface IUserDto
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    int UserId { get; }

    /// <summary>
    /// Adres email użytkownika.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Imię użytkownika.
    /// </summary>
    string FirstName { get; }

    /// <summary>
    /// Nazwisko użytkownika.
    /// </summary>
    string LastName { get; }

    /// <summary>
    /// Czy użytkownik ma uprawnienia administratora.
    /// </summary>
    bool IsAdmin { get; }
}
