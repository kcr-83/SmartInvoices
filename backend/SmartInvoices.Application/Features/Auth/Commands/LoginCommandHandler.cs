using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Auth;
using SmartInvoices.Application.Interfaces.Authentication;

namespace SmartInvoices.Application.Features.Auth.Commands;

/// <summary>
/// Handler przetwarzający komendę logowania.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public LoginCommandHandler(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    /// <summary>
    /// Obsługuje komendę logowania i generuje token JWT.
    /// </summary>
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.AuthenticateAsync(request.Email, request.Password);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Nieprawidłowe dane uwierzytelniające");
        }

        var token = GenerateJwtToken(user);
        var expiration = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"] ?? "60"));

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            User = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin
            }
        };
    }

    private JwtSecurityToken GenerateJwtToken(IUserDto user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("userId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["JwtSettings:Key"] ?? "defaultDevelopmentKey123!@#"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        return token;
    }
}
