using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoices.Application.Behaviors;
using SmartInvoices.Application.Common.Mediator;
using FluentValidation;
using Scrutor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SmartInvoices.Application;

/// <summary>
/// Klasa rozszerzająca konfigurację IoC dla warstwy aplikacji.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Rejestruje usługi warstwy aplikacji w kontenerze DI.
    /// </summary>
    /// <param name="services">Kolekcja usług</param>
    /// <returns>Zmodyfikowana kolekcja usług</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Rejestracja wszystkich handlerów na podstawie konwencji
        var assembly = Assembly.GetExecutingAssembly();

        // Rejestracja handlerów dla IRequestHandler<TRequest, TResponse>
        services.Scan(
            scan =>
                scan.FromAssemblyOf<IMediator>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
        );
        services.AddScoped<IMediator, SimpleMediator>();

        // Rejestracja behawiorów
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

 
        return services;
    }
}
