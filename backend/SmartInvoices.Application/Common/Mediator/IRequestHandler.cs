namespace SmartInvoices.Application.Common.Mediator;

/// <summary>
/// Interfejs dla obsługi żądań przetwarzanych przez mediator.
/// </summary>
/// <typeparam name="TRequest">Typ żądania</typeparam>
/// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Przetwarza żądanie i zwraca odpowiedź.
    /// </summary>
    /// <param name="request">Żądanie do przetworzenia</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Odpowiedź na żądanie</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
