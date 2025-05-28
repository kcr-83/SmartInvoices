using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoices.Application.Behaviors;
using SmartInvoices.Application.Common.Mediator;
using FluentValidation;

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
        // Rejestracja mediatora
        services.AddScoped<IMediator, MediatorWithBehaviors>();
        
        // Rejestracja behaviors dla mediatora
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Rejestracja handlerów komend i zapytań
        var assembly = Assembly.GetExecutingAssembly();
        RegisterHandlers(services, assembly);
        
        // Rejestracja walidatorów
        services.AddValidatorsFromAssembly(assembly); // Należy dodać pakiet FluentValidation.DependencyInjection
        
        return services;
    }
    
    /// <summary>
    /// Rejestruje handlery komend i zapytań w kontenerze DI.
    /// </summary>
    /// <param name="services">Kolekcja usług</param>
    /// <param name="assembly">Assembly zawierające handlery</param>
    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType && 
                        x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .ToList();

        foreach (var handler in handlerTypes)
        {
            var interfaceType = handler.Interface;
            services.AddTransient(interfaceType, handler.Type);
        }
    }
}
