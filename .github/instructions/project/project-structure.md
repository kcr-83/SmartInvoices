# Struktura Projektu: System Zarządzania Fakturami

Projekt jest zorganizowany zgodnie z zasadami Clean Architecture, obejmujący frontend (Angular), backend (.NET Core WebAPI) oraz warstwę danych. Poniżej przedstawiona jest szczegółowa struktura projektu.

## Struktura katalogów głównych

```
SmartInvoices/
├── frontend/                 # Aplikacja Angular
├── backend/                  # API .NET Core
├── database/                 # Skrypty SQL i migracje
├── docs/                     # Dokumentacja projektu
└── tools/                    # Narzędzia pomocnicze
```

## Frontend (Angular)

```
frontend/
├── src/
│   ├── app/
│   │   ├── core/                      # Usługi, modele i funkcje podstawowe
│   │   │   ├── models/                # Interfejsy i klasy modeli danych
│   │   │   │   ├── invoice.model.ts
│   │   │   │   ├── line-item.model.ts
│   │   │   │   ├── change-request.model.ts
│   │   │   │   ├── refund-request.model.ts
│   │   │   │   └── user.model.ts
│   │   │   ├── services/              # Usługi aplikacji
│   │   │   │   ├── auth.service.ts    # Usługa uwierzytelniania
│   │   │   │   ├── invoice.service.ts # Usługa obsługi faktur
│   │   │   │   ├── change-request.service.ts
│   │   │   │   └── refund-request.service.ts
│   │   │   └── guards/                # Strażnicy routingu
│   │   │       └── auth.guard.ts
│   │   ├── shared/                    # Współdzielone komponenty, dyrektywy i pipes
│   │   │   ├── components/
│   │   │   │   ├── header/
│   │   │   │   ├── footer/
│   │   │   │   ├── sidebar/
│   │   │   │   └── loading-spinner/
│   │   │   ├── directives/
│   │   │   └── pipes/
│   │   ├── features/                  # Moduły funkcjonalne
│   │   │   ├── auth/                  # Moduł uwierzytelniania
│   │   │   │   ├── login/
│   │   │   │   └── register/
│   │   │   ├── invoice-management/    # Moduł zarządzania fakturami
│   │   │   │   ├── invoice-list/
│   │   │   │   ├── invoice-detail/
│   │   │   │   └── invoice-filter/
│   │   │   ├── change-request/        # Moduł wniosków o zmiany
│   │   │   │   ├── change-request-form/
│   │   │   │   └── change-request-list/
│   │   │   └── refund-request/        # Moduł wniosków o zwroty
│   │   │       ├── refund-request-form/
│   │   │       └── refund-request-list/
│   │   └── admin/                     # Panel administratora
│   │       ├── user-management/
│   │       └── request-management/
│   ├── assets/                        # Zasoby statyczne
│   ├── environments/                  # Konfiguracje środowisk
│   │   ├── environment.ts
│   │   └── environment.prod.ts
│   ├── index.html
│   └── styles.scss
├── angular.json
├── package.json
└── tsconfig.json
```

## Backend (.NET Core WebAPI) - Clean Architecture

Backend jest zorganizowany zgodnie z zasadami Clean Architecture, z wyraźnym podziałem na warstwy zależności.

```
backend/
├── SmartInvoices.Domain/               # Warstwa Domain (Core Layer)
│   ├── Entities/                       # Encje domenowe
│   │   ├── Invoice.cs                  # Faktura
│   │   ├── InvoiceItem.cs              # Pozycje faktury
│   │   ├── ChangeRequest.cs            # Wnioski o zmiany
│   │   ├── RefundRequest.cs            # Wnioski o zwroty
│   │   └── User.cs                     # Użytkownicy
│   ├── ValueObjects/                   # Value Objects
│   │   └── Address.cs
│   ├── Events/                         # Zdarzenia domenowe
│   │   ├── InvoiceCreated.cs
│   │   └── RequestStatusChanged.cs
│   ├── Exceptions/                     # Wyjątki domenowe
│   │   ├── InvoiceNotFoundException.cs
│   │   └── InvalidOperationException.cs
│   ├── Interfaces/                     # Interfejsy repozytoriów
│   │   ├── IInvoiceRepository.cs
│   │   ├── IRequestRepository.cs
│   │   └── IUserRepository.cs
│   └── Enums/                          # Typy wyliczeniowe
│       ├── InvoiceStatus.cs
│       ├── RequestStatus.cs
│       └── RequestType.cs
│
├── SmartInvoices.Application/          # Warstwa Application (Core Layer)
│   ├── Features/                       # Funkcjonalności CQRS
│   │   ├── Invoices/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateInvoice/
│   │   │   │   │   ├── CreateInvoiceCommand.cs
│   │   │   │   │   └── CreateInvoiceCommandHandler.cs
│   │   │   │   └── UpdateInvoice/
│   │   │   │       ├── UpdateInvoiceCommand.cs
│   │   │   │       └── UpdateInvoiceCommandHandler.cs
│   │   │   └── Queries/
│   │   │       ├── GetInvoices/
│   │   │       │   ├── GetInvoicesQuery.cs
│   │   │       │   └── GetInvoicesQueryHandler.cs
│   │   │       └── GetInvoiceDetail/
│   │   │           ├── GetInvoiceDetailQuery.cs
│   │   │           └── GetInvoiceDetailQueryHandler.cs
│   │   ├── ChangeRequests/
│   │   │   ├── Commands/
│   │   │   └── Queries/
│   │   └── RefundRequests/
│   │       ├── Commands/
│   │       └── Queries/
│   ├── DTOs/                          # Obiekty transferu danych
│   │   ├── InvoiceDto.cs
│   │   ├── LineItemDto.cs
│   │   ├── ChangeRequestDto.cs
│   │   ├── RefundRequestDto.cs
│   │   └── UserDto.cs
│   ├── Interfaces/                    # Interfejsy usług infrastrukturalnych
│   │   ├── IEmailService.cs
│   │   ├── IPdfService.cs
│   │   └── IFileStorage.cs
│   ├── Validators/                    # Walidatory FluentValidation
│   │   ├── CreateInvoiceCommandValidator.cs
│   │   └── UpdateInvoiceCommandValidator.cs
│   ├── Behaviors/                     # Zachowania potoków mediatorów
│   │   ├── LoggingBehavior.cs
│   │   └── ValidationBehavior.cs
│   └── Common/                        # Wspólne abstrakcje
│       ├── Mediator/
│       │   ├── IRequest.cs
│       │   ├── IRequestHandler.cs
│       │   ├── IMediator.cs
│       │   └── SimpleMediator.cs
│       └── Result.cs
│
├── SmartInvoices.Persistence/          # Warstwa Persistence (Infrastructure Layer)
│   ├── SmartInvoicesDbContext.cs       # Główny kontekst bazy danych
│   ├── Configurations/                 # Konfiguracje encji
│   │   ├── InvoiceConfiguration.cs
│   │   ├── InvoiceItemConfiguration.cs
│   │   ├── ChangeRequestConfiguration.cs
│   │   ├── RefundRequestConfiguration.cs
│   │   └── UserConfiguration.cs
│   ├── Repositories/                   # Implementacje repozytoriów
│   │   ├── InvoiceRepository.cs
│   │   ├── ChangeRequestRepository.cs
│   │   ├── RefundRequestRepository.cs
│   │   └── UserRepository.cs
│   ├── Migrations/                     # Migracje bazy danych
│   └── DependencyInjection.cs          # Konfiguracja wstrzykiwania zależności dla warstwy
│
├── SmartInvoices.Infrastructure/       # Warstwa Infrastructure (Infrastructure Layer)
│   ├── Services/                       # Implementacje usług zewnętrznych
│   │   ├── EmailService.cs
│   │   ├── PdfService.cs
│   │   └── FileStorage.cs
│   ├── Security/                       # Bezpieczeństwo
│   │   ├── JwtHandler.cs
│   │   └── IdentityService.cs
│   ├── ExternalApis/                   # Integracje z zewnętrznymi API
│   └── DependencyInjection.cs          # Konfiguracja wstrzykiwania zależności dla warstwy
│
├── SmartInvoices.Common/               # Warstwa Common/Shared
│   ├── Extensions/                     # Metody rozszerzające
│   │   ├── StringExtensions.cs
│   │   └── DateTimeExtensions.cs
│   ├── Helpers/                        # Klasy pomocnicze
│   │   └── DateTimeHelper.cs
│   └── Middleware/                     # Middleware aplikacji
│       ├── ErrorHandlingMiddleware.cs
│       └── CorrelationIdMiddleware.cs
│
├── SmartInvoices.WebApi/               # Warstwa Prezentacji - API
│   ├── Controllers/                    # Kontrolery API
│   │   ├── InvoicesController.cs
│   │   ├── ChangeRequestsController.cs
│   │   ├── RefundRequestsController.cs
│   │   └── UsersController.cs
│   ├── Models/                         # Modele specyficzne dla API
│   │   └── ApiResponse.cs
│   ├── Middleware/                     # Middleware specyficzne dla API
│   │   └── ApiExceptionMiddleware.cs
│   ├── Program.cs                      # Punkt wejścia aplikacji
│   ├── appsettings.json                # Ustawienia aplikacji
│   └── DependencyInjection.cs          # Konfiguracja wstrzykiwania zależności
│
├── SmartInvoices.Service/              # Warstwa usług tła
│   ├── Program.cs                      # Punkt wejścia usługi
│   └── appsettings.json                # Ustawienia usługi
│
├── SmartInvoices.Service.Workers/      # Implementacje procesów w tle
│   ├── NotificationWorker.cs           # Worker do wysyłania powiadomień
│   ├── ReportGeneratorWorker.cs        # Worker do generowania raportów
│   ├── SyncWorker.cs                   # Worker do synchronizacji danych
│   └── Base/
│       └── Worker.cs                   # Klasa bazowa dla wszystkich workerów
│
└── Tests/
    ├── SmartInvoices.Domain.UnitTests/
    ├── SmartInvoices.Application.UnitTests/
    ├── SmartInvoices.Persistence.IntegrationTests/
    └── SmartInvoices.WebApi.IntegrationTests/
```

## Warstwa Danych

```
database/
├── scripts/                     # Skrypty SQL
│   ├── schema.sql               # Schemat bazy danych
│   └── seed-data.sql            # Dane początkowe
├── migrations/                  # Skrypty migracji
└── docker-compose.yml           # Konfiguracja bazy danych w kontenerze
```

## Dokumentacja

```
docs/
├── adr/                         # Architecture Decision Records
│   ├── 001-clean-architecture.md
│   └── 002-cqrs-pattern.md
├── architecture/                # Dokumentacja architektury
│   ├── diagrams/                # Diagramy UML, ERD itp.
│   └── decisions/               # Dokumenty decyzji architektonicznych
├── api/                         # Dokumentacja API
│   └── swagger/                 # Specyfikacja Swagger/OpenAPI
├── user-manuals/                # Podręczniki użytkownika
└── development-guides/          # Przewodniki dla programistów
    ├── setup.md                 # Konfiguracja środowiska deweloperskiego
    └── coding-standards.md      # Standardy kodowania
```

## Narzędzia

```
tools/
├── scripts/                     # Skrypty pomocnicze
│   ├── setup-dev.sh             # Skrypt konfiguracji środowiska deweloperskiego
│   └── deploy.sh                # Skrypt wdrożeniowy
└── monitoring/                  # Narzędzia monitorowania
    └── health-check.sh          # Skrypt sprawdzania stanu aplikacji
```

## Przykład Implementacji CQRS

Poniżej znajduje się przykład implementacji wzorca CQRS z użyciem SimpleMediator:

```csharp
// Command
public class CreateInvoiceCommand : IRequest<int>
{
    public string InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<InvoiceItemDto> Items { get; set; }
}

// Command Handler
public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, int>
{
    private readonly IInvoiceRepository _invoiceRepository;
    
    public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    
    public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = request.InvoiceNumber,
            IssueDate = request.IssueDate,
            TotalAmount = request.TotalAmount,
            Items = request.Items.Select(i => new InvoiceItem {
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        
        await _invoiceRepository.AddAsync(invoice, cancellationToken);
        return invoice.Id;
    }
}

// Controller using the mediator
[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateInvoiceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
```

## Przykład Worker Service

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
        _logger.LogInformation("Starting notification process");
        await _emailService.SendPendingNotificationsAsync(cancellationToken);
    }
}
```

## Podsumowanie

Powyższa struktura projektu została zaprojektowana z myślą o:

1. **Clean Architecture** - wyraźny podział na warstwy Domain, Application, Infrastructure i Presentation.
2. **CQRS Pattern** - rozdział operacji odczytu i zapisu przez Commands i Queries.
3. **Separacji warstw** - warstwy zewnętrzne zależą od warstw wewnętrznych, zgodnie z regułą zależności.
4. **Modularności** - organizacja kodu według funkcjonalności.
5. **Testowalności** - struktura wspierająca testowanie jednostkowe i integracyjne.
6. **Skalowalności** - łatwe rozszerzanie o nowe funkcjonalności.
7. **Prostoty wdrażania** - organizacja sprzyjająca procesom CI/CD.

Struktura ta uwzględnia wszystkie wymagania funkcjonalne systemu zarządzania fakturami zgodnie z wytycznymi Clean Architecture, zapewniając jednocześnie elastyczność i łatwość rozwoju w przyszłości.
