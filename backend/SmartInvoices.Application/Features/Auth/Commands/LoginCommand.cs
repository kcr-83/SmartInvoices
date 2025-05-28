using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Auth;

namespace SmartInvoices.Application.Features.Auth.Commands;

/// <summary>
/// Komenda do logowania użytkownika.
/// </summary>
public class LoginCommand : IRequest<AuthResponseDto>
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
