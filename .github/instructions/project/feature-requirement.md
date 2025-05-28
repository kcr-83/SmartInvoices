# System Zarządzania Fakturami - Wymagania Funkcjonalne

## Opis systemu
System zarządzania fakturami zapewniający użytkownikom możliwość przeglądania faktur, wnioskowania o zmiany w pozycjach oraz składania wniosków o pełne zwroty. System będzie zbudowany w oparciu o zasady Clean Architecture, wykorzystując .NET Core WebAPI dla usług backendu oraz Angular dla interfejsu użytkownika. Architektura systemu będzie oparta o warstwy:

```
Domain → Application → Infrastructure/Presentation
```

## Epiki

### Epik 1: Zarządzanie Fakturami
Jako użytkownik systemu, chcę mieć możliwość efektywnego zarządzania moimi fakturami, aby kontrolować moje wydatki i zarządzać płatnościami.

### Epik 2: System Administracyjny
Jako administrator systemu, chcę mieć możliwość zarządzania użytkownikami i fakturami, aby zapewnić prawidłowe funkcjonowanie systemu.

### Epik 3: Bezpieczeństwo i Zgodność
Jako właściciel systemu, chcę zapewnić bezpieczeństwo danych oraz zgodność z przepisami, aby chronić dane użytkowników i uniknąć konsekwencji prawnych.

## Funkcje i Historie Użytkownika

### Funkcja 1.1: Przeglądanie Faktur
Umożliwienie użytkownikom wglądu do wszystkich swoich faktur i ich szczegółów.

**Historie użytkownika:**
1. Jako użytkownik, chcę przeglądać listę wszystkich moich faktur, aby mieć ogólny widok moich transakcji.
2. Jako użytkownik, chcę filtrować i sortować faktury według różnych kryteriów (data, kwota, status), aby szybko znaleźć interesującą mnie fakturę.
3. Jako użytkownik, chcę zobaczyć szczegóły wybranej faktury, aby poznać wszystkie informacje dotyczące transakcji.
4. Jako użytkownik, chcę mieć możliwość eksportu faktury do PDF, aby zachować ją lokalnie lub wydrukować.

### Funkcja 1.2: Wnioski o Zmiany w Pozycjach Faktury
Umożliwienie użytkownikom wnioskowania o zmiany w pozycjach na fakturach.

**Historie użytkownika:**
1. Jako użytkownik, chcę zaznaczyć konkretne pozycje na fakturze do zmiany, aby wskazać które elementy wymagają korekty.
2. Jako użytkownik, chcę określić rodzaj zmiany dla wybranych pozycji (zmiana ilości, ceny, opisu), aby sprecyzować swoje oczekiwania.
3. Jako użytkownik, chcę dodać uzasadnienie do wniosku o zmianę, aby wyjaśnić powód mojej prośby.
4. Jako użytkownik, chcę śledzić status mojego wniosku o zmianę, aby wiedzieć czy został zaakceptowany.

### Funkcja 1.3: Wnioski o Pełne Zwroty
Umożliwienie użytkownikom wnioskowania o pełne zwroty za faktury.

**Historie użytkownika:**
1. Jako użytkownik, chcę złożyć wniosek o pełny zwrot za fakturę, aby odzyskać pełną kwotę transakcji.
2. Jako użytkownik, chcę podać powód zwrotu, aby uzasadnić moją prośbę.
3. Jako użytkownik, chcę dołączyć dowody/dokumentację do wniosku o zwrot, aby zwiększyć szansę na pozytywne rozpatrzenie.
4. Jako użytkownik, chcę śledzić status mojego wniosku o zwrot, aby wiedzieć na jakim etapie się znajduje.

### Funkcja 2.1: Zarządzanie Użytkownikami
Umożliwienie administratorom zarządzania kontami użytkowników.

**Historie użytkownika:**
1. Jako administrator, chcę dodawać nowych użytkowników do systemu, aby zapewnić im dostęp.
2. Jako administrator, chcę modyfikować uprawnienia użytkowników, aby kontrolować ich poziom dostępu do funkcji systemu.
3. Jako administrator, chcę dezaktywować konta użytkowników, aby uniemożliwić dostęp nieaktywnym użytkownikom.

### Funkcja 2.2: Zarządzanie Wnioskami
Umożliwienie administratorom przetwarzania wniosków o zmiany i zwroty.

**Historie użytkownika:**
1. Jako administrator, chcę przeglądać wszystkie wnioski o zmiany i zwroty, aby mieć wgląd w aktywność użytkowników.
2. Jako administrator, chcę akceptować lub odrzucać wnioski użytkowników, aby rozstrzygać ich żądania.
3. Jako administrator, chcę dodawać komentarze do wniosków, aby informować użytkowników o decyzjach i powodach.

## Techniczne Aspekty Implementacji

### Struktura Rozwiązania
Rozwiązanie systemu SmartInvoices będzie podzielone na następujące projekty zgodnie z zasadami Clean Architecture:

### Core Layers

#### Domain Layer Projects (SmartInvoices.Domain)
- **Purpose**: Core business entities and logic
- **Content**:
  - Entity classes with business logic:
    - Faktury (Invoice)
    - Pozycje faktur (InvoiceItem)
    - Wnioski o zmiany (ChangeRequest)
    - Wnioski o zwroty (RefundRequest)
    - Użytkownicy (User)
  - Value objects
  - Domain events (InvoiceCreated, RequestStatusChanged)
  - Domain exceptions
  - Core interfaces (repository interfaces: IInvoiceRepository, IRequestRepository, IUserRepository)
  - Enums and constants
- **Guidelines**:
  - No dependencies on other layers
  - No reference to infrastructure concerns
  - Keep entities focused on business rules

#### Application Layer Projects (SmartInvoices.Application)
- **Purpose**: Use cases, application logic, DTOs
- **Content**:
  - Command and query handlers (CQRS):
    - Komendy i obsługa komend (np. CreateInvoiceCommand, ProcessRefundRequestCommand)
    - Zapytania i obsługa zapytań (np. GetInvoicesQuery, GetRequestDetailsQuery)
  - DTOs (Data Transfer Objects)
  - Interfaces for infrastructure services (IEmailService, IPdfService)
  - Validation logic with FluentValidation
  - Mapping profiles (do not use AutoMapper because it's paid, use custom mapping inside model classes using static methods)
- **Guidelines**:
  - Depend only on the domain layer
  - Use CQRS pattern with commands and queries
  - Implement business rules that span multiple entities

### Infrastructure Layers

#### Persistence Layer Projects (SmartInvoices.Persistence)
- **Purpose**: Data access and storage
- **Content**:
  - DbContext classes (SmartInvoicesDbContext)
  - Repository implementations (InvoiceRepository, RequestRepository, UserRepository)
  - Entity configurations
  - Migrations
  - Model danych przechowujący:
    - Dane użytkowników
    - Faktury i ich pozycje
    - Wnioski o zmiany
    - Wnioski o zwroty
    - Historię operacji
- **Guidelines**:
  - Implement repository interfaces from domain layer
  - Use EF Core for ORM
  - Separate configurations from entity classes

#### Infrastructure Layer Projects (SmartInvoices.Infrastructure)
- **Purpose**: External services, cross-cutting concerns
- **Content**:
  - External service integrations
  - File handling
  - Email services (EmailService)
  - PDF generation (PdfService)
  - Authentication and authorization services
- **Guidelines**:
  - Implement interfaces defined in application layer
  - Keep external service logic isolated
  - Use dependency injection for service registration

#### Common/Shared Projects (SmartInvoices.Common)
- **Purpose**: Utilities and helpers
- **Content**:
  - Extension methods
  - Helpers
  - Middleware
  - Serialization utilities
- **Guidelines**:
  - Keep focused on reusable utilities
  - Avoid business logic

### Presentation Layer

#### API Projects (SmartInvoices.WebApi)
- **Purpose**: User-facing services
- **Content**:
  - Controllers/API endpoints:
    - InvoicesController
    - ChangeRequestsController
    - RefundRequestsController
    - UsersController
  - Startup configuration
  - API-specific models
  - Swagger/OpenAPI configuration
  - Error handling middleware
  - CORS and security configuration
- **Guidelines**:
  - Keep controllers thin
  - Use mediator pattern to delegate to application layer

#### Worker Service Projects (SmartInvoices.Service, SmartInvoices.Service.Workers)
- **Purpose**: Background processing
- **Content**:
  - Worker services for:
    - Wysyłanie powiadomień email
    - Generowanie raportów okresowych
    - Synchronizacja danych z zewnętrznymi systemami
- **Guidelines**:
  - Inherit from BackgroundService or custom Worker base class
  - Follow single responsibility principle
  - Use dependency injection for services

### Frontend (Angular)
- **Purpose**: User interface
- **Content**:
  - Responsive user interface
  - Modules for invoice management, change requests, and refund requests
  - Form components for submitting requests
  - Data filtering and sorting mechanisms
  - Invoice detail views
  - Request status panels

## Code Organization Patterns

### CQRS Pattern
When implementing features:
1. Create a command/query class in the appropriate folder
2. Implement a handler class that processes the command/query
3. Do not use MediatR to dispatch commands/queries (because it's paid), use custom implementation
4. Use dependency injection to register handlers

Example implementation of SimpleMediator for CQRS:
```csharp
public interface IRequest<out TResponse> { }

public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IMediator
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;
}

public class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await handler.Handle(request, cancellationToken);
    }
}
```

### Worker Pattern
For background services:
1. Create a worker class that inherits from the Worker base class
2. Implement the PerformWorkAsync method
3. Register the worker in dependency injection

Example:
```csharp
public class NotificationWorker : Worker
{
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationWorker> _logger;

    public NotificationWorker(
        IEmailService emailService,
        ILogger<NotificationWorker> logger,
        IWorkerOptions options,
        IServiceScopeFactory serviceScopeFactory)
        : base(options, logger, serviceScopeFactory)
    {
        _emailService = emailService;
        _logger = logger;
    }

    protected override async Task PerformWorkAsync(CancellationToken cancellationToken)
    {
        // Worker implementation for sending notifications
        _logger.LogInformation("Starting notification process");
        await _emailService.SendPendingNotificationsAsync(cancellationToken);
    }
}
```

### Dependency Injection
For registering services:
1. Create extension methods in DependencyInjection.cs files
2. Group related services together
3. Use appropriate lifetime scopes (Singleton, Scoped, Transient)

Example:
```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IMediator, SimpleMediator>();
        
        // Register all command and query handlers
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddHandlers(assembly);
        
        return services;
    }
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddTransient<IFileStorage, FileStorage>();
        
        return services;
    }
}
```

## Common File Locations

When looking for specific files or adding new ones:

### Domain Models
- Location: `SmartInvoices.Domain/Entities/`
- Example: `Invoice.cs`, `InvoiceItem.cs`, `ChangeRequest.cs`

### Application Commands/Queries
- Location: `SmartInvoices.Application/Features/{Feature}/Commands/` or `SmartInvoices.Application/Features/{Feature}/Queries/`
- Example: `CreateInvoiceCommand.cs`, `GetInvoicesQuery.cs`

### DbContext
- Location: `SmartInvoices.Persistence/`
- Main file: `SmartInvoicesDbContext.cs`

### Entity Configurations
- Location: `SmartInvoices.Persistence/Configurations/`
- Example: `InvoiceConfiguration.cs`

### Worker Services
- Location: `SmartInvoices.Service.Workers/{WorkerType}/`
- Example: `NotificationWorker.cs`, `ReportGeneratorWorker.cs`

### API Controllers
- Location: `SmartInvoices.WebApi/Controllers/`
- Example: `InvoicesController.cs`, `RequestsController.cs`

## Testing Guidelines

### Unit Tests
- Name test projects with `.UnitTests` suffix (e.g., `SmartInvoices.Application.UnitTests`)
- Structure test files to match the structure of the code being tested
- Use test class naming convention: `{ClassUnderTest}Tests`
- Use test method naming convention: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

### Integration Tests
- Name test projects with `.IntegrationTests` suffix
- Use in-memory database for testing repositories
- Mock external services
- Create test fixtures for common setup logic

## Error Handling and Other Practices

### Error Handling
- Use domain exceptions for business rule violations
- Use middleware for global exception handling in APIs
- Log exceptions appropriately
- Use Result pattern for error handling in application layer

### Validation
- Use FluentValidation for input validation
- Implement validation behavior with mediator pipeline
- Return validation errors as part of API response

### Logging
- Use structured logging with Serilog
- Log appropriate levels (Debug, Information, Warning, Error)
- Include correlation IDs for tracking requests

### Integracje
- System powiadomień (email/wewnętrzny)
- Możliwość eksportu danych do różnych formatów (PDF, CSV)
- Integracja z systemami płatności (opcjonalnie)
