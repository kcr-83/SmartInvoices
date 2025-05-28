namespace SmartInvoices.Application.Common.Mediator;

/// <summary>
/// Interfejs bazowy dla żądań przetwarzanych przez mediator.
/// </summary>
/// <typeparam name="TResponse">Typ zwracany przez żądanie</typeparam>
public interface IRequest<out TResponse>
{
}
