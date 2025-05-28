using Microsoft.Extensions.DependencyInjection;

namespace SmartInvoices.Application.Common.Mediator;

/// <summary>
/// Prosta implementacja mediatora obsługującego komunikację między komponentami aplikacji.
/// </summary>
public class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Wysyła żądanie do odpowiedniego handlera i zwraca rezultat.
    /// </summary>
    /// <typeparam name="TRequest">Typ żądania</typeparam>
    /// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
    /// <param name="request">Żądanie do przetworzenia</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Odpowiedź na żądanie</returns>
    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await handler.Handle(request, cancellationToken);
    }
}
