using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoices.Application.Interfaces;
using SmartInvoices.Application.Interfaces.Authentication;
using SmartInvoices.Infrastructure.Authentication;
using SmartInvoices.Infrastructure.FileStorage;
using SmartInvoices.Infrastructure.Services;

namespace SmartInvoices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Rejestracja usług infrastruktury
        services.AddScoped<IFileStorage, LocalFileStorage>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();


        return services;
    }
}
