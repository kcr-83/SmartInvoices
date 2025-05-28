using FastEndpoints;
using FluentValidation;

namespace SmartInvoices.WebApi.Endpoints.Auth.Login;

/// <summary>
/// Walidator dla żądania logowania.
/// </summary>
public class LoginValidator : Validator<LoginRequest>
{
    /// <summary>
    /// Konstruktor definiujący reguły walidacji.
    /// </summary>
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Adres email jest wymagany")
            .EmailAddress().WithMessage("Podany adres email ma niepoprawny format");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Hasło jest wymagane")
            .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków");
    }
}
