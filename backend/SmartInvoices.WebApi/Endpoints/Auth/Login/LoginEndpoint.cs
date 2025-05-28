using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Features.Auth.Commands;

namespace SmartInvoices.WebApi.Endpoints.Auth.Login;

/// <summary>
/// Endpoint logowania użytkownika.
/// </summary>
[AllowAnonymous]
public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public LoginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Konfiguracja endpointu.
    /// </summary>
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
        Description(d => 
        {
            d.WithName("Login");
            d.WithTags("Auth");
            d.WithDescription("Uwierzytelnia użytkownika i zwraca token JWT");
            d.ProducesProblem(401);
        });
        Summary(s =>
        {
            s.Summary = "Logowanie użytkownika";
            s.Description = "Endpoint do uwierzytelniania użytkownika i generowania tokenu JWT";
            s.ResponseExamples[200] = new LoginResponse
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                Expiration = DateTime.UtcNow.AddHours(1),
                User = new UserResponse
                {
                    UserId = 1,
                    Email = "user@example.com",
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    IsAdmin = false
                }
            };
        });
    }

    /// <summary>
    /// Obsługa żądania logowania.
    /// </summary>
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        try
        {
            var command = new LoginCommand
            {
                Email = req.Email,
                Password = req.Password
            };

            var result = await _mediator.Send<LoginCommand, Application.DTOs.Auth.AuthResponseDto>(command, ct);

            var response = new LoginResponse
            {
                Token = result.Token,
                Expiration = result.Expiration,
                User = new UserResponse
                {
                    UserId = result.User.UserId,
                    Email = result.User.Email,
                    FirstName = result.User.FirstName,
                    LastName = result.User.LastName,
                    IsAdmin = result.User.IsAdmin
                }
            };

            await SendAsync(response, 200, ct);
        }
        catch (UnauthorizedAccessException)
        {
            await SendUnauthorizedAsync(ct);
        }
    }
}
