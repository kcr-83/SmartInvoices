namespace SmartInvoices.Application.DTOs;

/// <summary>
/// Data Transfer Object dla użytkownika.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nazwa użytkownika.
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Adres e-mail użytkownika.
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
    /// Role użytkownika.
    /// </summary>
    public List<string> Roles { get; set; } = new List<string>();
    
    /// <summary>
    /// Flaga wskazująca, czy konto użytkownika jest aktywne.
    /// </summary>
    public bool IsActive { get; set; }
}
