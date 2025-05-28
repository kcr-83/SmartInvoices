using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Behaviors;

/// <summary>
/// Interfejs dla zachowania potoków mediatorów.
/// </summary>
/// <typeparam name="TRequest">Typ żądania</typeparam>
/// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Obsługuje żądanie w potoku mediatorów.
    /// </summary>
    /// <param name="request">Żądanie do przetworzenia</param>
    /// <param name="next">Następny handler w potoku</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Odpowiedź na żądanie</returns>
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

/// <summary>
/// Delegat do obsługi żądań w potoku mediatorów.
/// </summary>
/// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
/// <returns>Zadanie zwracające odpowiedź</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
