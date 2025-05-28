using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Behaviors;

/// <summary>
/// Rozszerzenie klasy SimpleMediator, które umożliwia obsługę zachowań potoków.
/// </summary>
public class MediatorWithBehaviors : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public MediatorWithBehaviors(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }    /// <summary>
    /// Wysyła żądanie do odpowiedniego handlera przez potok zachowań.
    /// </summary>
    /// <typeparam name="TRequest">Typ żądania</typeparam>
    /// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
    /// <param name="request">Żądanie do przetworzenia</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Odpowiedź na żądanie</returns>
    public async Task<TResponse> Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default
    ) where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetService(typeof(IRequestHandler<TRequest, TResponse>)) 
            as IRequestHandler<TRequest, TResponse>
            ?? throw new InvalidOperationException(
                $"Handler dla {typeof(TRequest).Name} nie został zarejestrowany."
            );

        // Pobierz zachowania potoku
        var behaviors = _serviceProvider.GetServices(typeof(IPipelineBehavior<TRequest, TResponse>)) 
            as IEnumerable<IPipelineBehavior<TRequest, TResponse>>
            ?? Enumerable.Empty<IPipelineBehavior<TRequest, TResponse>>();

        // Utwórz delegata obsługi żądania
        Task<TResponse> Handle() => handler.Handle(request, cancellationToken);

        // Zbuduj potok z zachowań w odwrotnej kolejności (aby wykonać je w odpowiedniej kolejności)
        var pipeline = behaviors.Reverse()
            .Aggregate(
                (RequestHandlerDelegate<TResponse>)Handle,
                (next, behavior) => () => behavior.Handle(request, next, cancellationToken)
            );

        // Wykonaj potok
        return await pipeline();
    }
}
