using Microsoft.Extensions.Logging;
using SmartInvoices.Application.DTOs.Auth;
using SmartInvoices.Application.Interfaces.Authentication;

namespace SmartInvoices.Infrastructure.Authentication;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }

    public Task<AuthResponseDto?> AuthenticateAsync(string username, string password)
    {
        // W rzeczywistej implementacji sprawdzilibyśmy dane logowania w bazie danych
        // i wygenerowali token JWT
        if (username == "admin" && password == "admin")
        {
            _logger.LogInformation("Pomyślnie zalogowano użytkownika {Username}", username);

            return Task.FromResult<AuthResponseDto?>(
                new AuthResponseDto
                {
                    Token = "sample-jwt-token",
                    Expiration = DateTime.Now.AddSeconds(3600), // Token ważny przez 1 godzinę
                    User = new UserDto
                    {
                        UserId = 1,
                        Email = "admin@example.com",
                        FirstName = username.Split(' ')[0].ToUpper(),
                        LastName = username.Split(' ')[1].ToUpper(),
                        IsAdmin = true
                    }
                }
            );
        }

        _logger.LogWarning("Nieudana próba logowania dla użytkownika {Username}", username);
        return Task.FromResult<AuthResponseDto?>(null);
    }

    public Task<IUserDto?> GetByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    Task<IUserDto?> IUserService.AuthenticateAsync(string email, string password)
    {
        throw new NotImplementedException();
    }
}
