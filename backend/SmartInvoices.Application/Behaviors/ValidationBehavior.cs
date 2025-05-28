using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Behaviors;

/// <summary>
/// Zachowanie potoku do walidacji żądań przed ich obsługą.
/// </summary>
/// <typeparam name="TRequest">Typ żądania</typeparam>
/// <typeparam name="TResponse">Typ odpowiedzi</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Konstruktor ValidationBehavior.
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="validators">Lista walidatorów dla żądania</param>
    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    /// <summary>
    /// Obsługuje żądanie, wykonując najpierw walidację.
    /// </summary>
    /// <param name="request">Żądanie do przetworzenia</param>
    /// <param name="next">Następny handler w potoku</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Odpowiedź na żądanie</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        _logger.LogWarning("Validation errors occurred in request {RequestType}", typeof(TRequest).Name);
        
        var errors = failures.Select(failure => $"{failure.PropertyName}: {failure.ErrorMessage}").ToList();
        
        if (typeof(TResponse).IsGenericType && 
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            // Jeśli TResponse to Result<T>, utwórz obiekt Result.Failure
            var resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GenericTypeArguments[0]);
            var failureMethod = resultType.GetMethod("Failure", new[] { typeof(List<string>) });
            
            if (failureMethod != null)
            {
                return (TResponse)failureMethod.Invoke(null, new object[] { errors })!;
            }
        }
        
        // Jeśli TResponse nie jest typu Result<T>, zgłaszamy wyjątek
        throw new ValidationException($"Validation failed: {string.Join(", ", errors)}");
    }
}

/// <summary>
/// Interfejs walidatora dla żądań.
/// </summary>
/// <typeparam name="T">Typ obiektu do walidacji</typeparam>
public interface IValidator<T>
{
    /// <summary>
    /// Waliduje obiekt.
    /// </summary>
    /// <param name="context">Kontekst walidacji</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Wynik walidacji</returns>
    Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellationToken);
}

/// <summary>
/// Kontekst walidacji.
/// </summary>
/// <typeparam name="T">Typ obiektu do walidacji</typeparam>
public class ValidationContext<T>
{
    /// <summary>
    /// Obiekt do walidacji.
    /// </summary>
    public T Instance { get; }

    /// <summary>
    /// Konstruktor kontekstu walidacji.
    /// </summary>
    /// <param name="instance">Obiekt do walidacji</param>
    public ValidationContext(T instance)
    {
        Instance = instance;
    }
}

/// <summary>
/// Wynik walidacji.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Lista błędów walidacji.
    /// </summary>
    public List<ValidationFailure> Errors { get; } = new List<ValidationFailure>();
    
    /// <summary>
    /// Flaga wskazująca, czy walidacja zakończyła się sukcesem.
    /// </summary>
    public bool IsValid => !Errors.Any();
}

/// <summary>
/// Pojedynczy błąd walidacji.
/// </summary>
public class ValidationFailure
{
    /// <summary>
    /// Nazwa właściwości, która nie przeszła walidacji.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Komunikat błędu.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
}

/// <summary>
/// Wyjątek zgłaszany w przypadku błędów walidacji.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Konstruktor wyjątku ValidationException.
    /// </summary>
    /// <param name="message">Komunikat błędu</param>
    public ValidationException(string message) : base(message)
    {
    }
}
