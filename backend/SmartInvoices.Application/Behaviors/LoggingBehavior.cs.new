using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartInvoices.Application.Behaviors
{
    /// <summary>
    /// Klasa implementująca zachowanie do logowania żądań i odpowiedzi mediatora.
    /// </summary>
    /// <typeparam name="TRequest">Typ żądania</typeparam>
    /// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Konstruktor LoggingBehavior.
        /// </summary>
        /// <param name="logger">Logger</param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obsługuje żądanie, logując jego szczegóły.
        /// </summary>
        /// <param name="request">Żądanie do przetworzenia</param>
        /// <param name="next">Następny handler w potoku</param>
        /// <param name="cancellationToken">Token anulowania</param>
        /// <returns>Odpowiedź na żądanie</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            
            _logger.LogInformation("Handling {RequestName}", requestName);
            
            try
            {
                var response = await next();
                
                _logger.LogInformation("Handled {RequestName}", requestName);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {RequestName}", requestName);
                throw;
            }
        }
    }
}
