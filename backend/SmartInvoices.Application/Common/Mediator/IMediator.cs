using Microsoft.Extensions.DependencyInjection;

namespace SmartInvoices.Application.Common.Mediator;

/// <summary>
/// Interfejs mediatora obsługującego komunikację między komponentami aplikacji.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Wysyła żądanie do odpowiedniego handlera i zwraca rezultat.
    /// </summary>
    /// <typeparam name="TRequest">Typ żądania</typeparam>
    /// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
    /// <param name="request">Żądanie do przetworzenia</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Odpowiedź na żądanie</returns>
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;
}
