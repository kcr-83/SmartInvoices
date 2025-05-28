# GitHub Copilot Guidance for PDF Extractor Architecture

This document serves as a reference for GitHub Copilot when working with projects based on the .Net Core application. It provides guidance on the project structure, code organization, and common patterns to follow. "PdfExtractor" is the name of example project it should be replaced with the actual project name.

## General Architecture Understanding

The PDF Extractor solution follows Clean Architecture principles with the following layers:

```
Domain → Application → Infrastructure/Presentation
```

When working with this architecture:
- Follow the dependency rule: dependencies only point inward
- Keep domain models free of infrastructure concerns
- Use interfaces to abstract infrastructure implementations
- Place cross-cutting concerns in appropriate layers

## Project Structure Guidelines

### Core Layers

#### Domain Layer Projects
- **Purpose**: Core business entities and logic
- **Naming**: `*.Domain` (e.g., `PdfExtractor.Domain`)
- **Content**:
  - Entity classes with business logic
  - Value objects
  - Domain events
  - Domain exceptions
  - Core interfaces (repository interfaces)
  - Enums and constants
- **Guidelines**:
  - No dependencies on other layers
  - No reference to infrastructure concerns
  - Keep entities focused on business rules

#### Application Layer Projects
- **Purpose**: Use cases, application logic, DTOs
- **Naming**: `*.Application` (e.g., `PdfExtractor.Application`)
- **Content**:
  - Command and query handlers (CQRS without MediatR for decoupling request handling and simplifying cross-cutting concerns, because is paid, use own implementation)
  - DTOs (Data Transfer Objects)
  - Interfaces for infrastructure services
  - Validation logic
  - Mapping profiles (do not use AutoMapper because it's paid, use custom mapping inside model classes using static methods)
- **Guidelines**:
  - Depend only on the domain layer
  - Use CQRS pattern with commands and queries
  - Implement business rules that span multiple entities

### Infrastructure Layers

#### Persistence Layer Projects
- **Purpose**: Data access and storage
- **Naming**: `*.Persistence` (e.g., `PdfExtractor.Persistence`)
- **Content**:
  - DbContext classes
  - Repository implementations
  - Entity configurations
  - Migrations
- **Guidelines**:
  - Implement repository interfaces from domain layer
  - Use EF Core for ORM
  - Separate configurations from entity classes

#### Infrastructure Layer Projects
- **Purpose**: External services, cross-cutting concerns
- **Naming**: `*.Infrastructure` (e.g., `PdfExtractor.Infrastructure`)
- **Content**:
  - External service integrations
  - File handling
  - Email services
  - Authentication services
- **Guidelines**:
  - Implement interfaces defined in application layer
  - Keep external service logic isolated
  - Use dependency injection for service registration

#### Common/Shared Projects
- **Purpose**: Utilities and helpers
- **Naming**: `*.Common`, `*.Shared.Common`
- **Content**:
  - Extension methods
  - Helpers
  - Middleware
  - Serialization utilities
- **Guidelines**:
  - Keep focused on reusable utilities
  - Avoid business logic

### Presentation Layer

#### API Projects
- **Purpose**: User-facing services
- **Naming**: `*.WebApi`, `*.AuthApi`
- **Content**:
  - Controllers/API endpoints
  - Startup configuration
  - API-specific models
- **Guidelines**:
  - Keep controllers thin
  - Use custom mediator implementation to delegate to application layer

### Worker Service Projects
- **Purpose**: Background processing
- **Naming**: `*.Service`, `*.Service.Workers`
- **Content**:
  - Worker services
  - Background processing logic
- **Guidelines**:
  - Inherit from BackgroundService or custom Worker base class
  - Follow single responsibility principle
  - Use dependency injection for services

## Code Organization Patterns

### CQRS Pattern
When implementing features:
1. Create a command/query class in the appropriate folder
2. Implement a handler class that processes the command/query
3. Do not use MediatR to dispatch commands/queries, becouse is paid, use custom implementation
4. Use dependency injection to register handlers

Example:
```csharp
//customMediator
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace YourNamespace.Mediator
{
    // Core interfaces
    public interface IRequest<out TResponse> { }
    
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    // Optional: Pipeline behavior (like middleware)
    public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
    
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

    // The mediator implementation
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handler = _serviceProvider.GetService(handlerType) 
                ?? throw new InvalidOperationException($"No handler registered for {requestType.Name}");

            // Get pipeline behaviors
            var behaviors = _serviceProvider.GetServices<IPipelineBehavior<IRequest<TResponse>, TResponse>>();
            
            // Create the handler execution pipeline
            Task<TResponse> Handle() => (Task<TResponse>)handlerType
                .GetMethod("Handle")
                ?.Invoke(handler, new object[] { request, cancellationToken })
                ?? throw new InvalidOperationException($"Handler for {requestType.Name} does not implement Handle method correctly");

            // Apply behaviors in reverse order (so they execute in the correct order)
            var pipeline = behaviors.Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handle, 
                    (next, behavior) => () => behavior.Handle(request, next, cancellationToken));

            // Execute the pipeline
            return await pipeline();
        }
    }
    
    // The mediator interface
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
    
    // Extension methods for service registration
    public static class MediatorServiceExtensions
    {
        public static IServiceCollection AddCustomMediator(this IServiceCollection services, System.Reflection.Assembly[] assemblies)
        {
            // Register the mediator
            services.AddScoped<IMediator, Mediator>();
            
            // Register handlers by convention
            foreach (var assembly in assemblies)
            {
                var handlerTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                    .Where(x => x.Interface.IsGenericType && 
                                x.Interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .ToList();

                foreach (var handler in handlerTypes)
                {
                    var interfaceType = handler.Interface;
                    services.AddTransient(interfaceType, handler.Type);
                }
            }
            
            return services;
        }
    }
}
// Command
public class ProcessDocumentCommand : IRequest<int>
{
    public int DocumentId { get; set; }
    public byte[] Content { get; set; }
}

// Handler
public class ProcessDocumentCommandHandler : IRequestHandler<ProcessDocumentCommand, int>
{
    public async Task<int> Handle(ProcessDocumentCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// Define requests and handlers
public record GetUserQuery(int UserId) : IRequest<UserDto>;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _repository;
    
    public GetUserQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);
        return new UserDto(user.Id, user.Name);
    }
}

// Using the mediator
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _mediator.Send(new GetUserQuery(id));
        return Ok(user);
    }
}

//SimpleMediator
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
public class SampleWorker : Worker
{
    public SampleWorker(
        ILogger logger,
        IOptions<PdfExtractorServiceConfiguration> options,
        IServiceScopeFactory serviceScopeFactory)
        : base(options, logger, serviceScopeFactory)
    {
    }

    protected override async Task PerformWorkAsync(CancellationToken cancellationToken)
    {
        // Worker implementation
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
public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration configuration)
{
    services.AddScoped<IMyService, MyService>();
    services.AddTransient<IMyTransientService, MyTransientService>();
    services.AddSingleton<IMySingletonService, MySingletonService>();
    return services;
}
```

## Common File Locations

When looking for specific files or adding new ones:

### Domain Models
- Location: `PdfExtractor.Domain/Entities/`
- Example: `Document.cs`, `Field.cs`, `Pattern.cs`

### Application Commands/Queries
- Location: `PdfExtractor.Application/{Feature}/Commands/` or `PdfExtractor.Application/{Feature}/Queries/`
- Example: `ProcessDocumentCommand.cs`, `GetDocumentQuery.cs`

### DbContext
- Location: `PdfExtractor.Persistence/`
- Main file: `PdfExtractorDbContext.cs`

### Entity Configurations
- Location: `PdfExtractor.Persistence/Configurations/`
- Example: `DocumentConfiguration.cs`

### Worker Services
- Location: `PdfExtractor.Service.Workers/{WorkerType}/`
- Example: `OcrWorker.cs`, `ImporterWorker.cs`

### API Controllers
- Location: `PdfExtractor.WebApi/Controllers/`
- Example: `DocumentsController.cs`

## Testing Guidelines

### Unit Tests
- Name test projects with `.UnitTests` suffix
- Structure test files to match the structure of the code being tested
- Use test class naming convention: `{ClassUnderTest}_Tests`
- Use test method naming convention: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

### Integration Tests
- Name test projects with `.IntegrationTests` suffix
- Use in-memory database for testing repositories
- Mock external services
- Create test fixtures for common setup logic

## Common Practices

### Error Handling
- Use domain exceptions for business rule violations
- Use middleware for global exception handling in APIs
- Log exceptions appropriately
- Use Result pattern for error handling in application layer

### Validation
- Use FluentValidation for input validation
- Implement validation behavior without MediatR for decoupling request handling and simplifying cross-cutting concerns, because is paid, use own implementation pipeline
- Return validation errors as part of API response

### Logging
- Use structured logging with Serilog
- Log appropriate levels (Debug, Information, Warning, Error)
- Include correlation IDs for tracking requests

### Configuration
- Use IOptions pattern for strongly typed configuration
- Separate configuration into logical sections
- Support environment-specific configuration

## When Extending the System

### Adding a New Entity
1. Create entity class in Domain layer
2. Add DbSet to IPdfExtractorDbContext and implement in PdfExtractorDbContext
3. Create entity configuration
4. Create migration
5. Implement repository if needed
6. Create commands/queries and handlers

### Adding a New Worker
1. Create worker class in Service.Workers project
2. Implement worker logic
3. Register worker in DependencyInjection
4. Add configuration in appsettings.json

### Adding a New API Endpoint
1. Create command/query in Application layer
2. Implement handler
3. Create controller method in API project
4. Configure routing

### Adding a New Feature
1. Identify domain concepts
2. Update domain model if needed
3. Create application services (commands/queries)
4. Implement infrastructure requirements
5. Create API endpoints or worker processes
6. Write tests

### Key Technologies and Components

- **Backend**:
  - .NET 9 Web API projects
  - Authentication handled by AuthApi
  - Clean architecture with separated layers (Domain, Application, Infrastructure, Persistence)
  - Entity Framework Core for database access

- **Documentation and API**:
  - Swagger/OpenAPI for API documentation
  - Multiple API endpoints with versioning

- **Testing**:
  - Unit tests with in-memory database support
  - Mock services and database contexts

- **Architecture Pattern**:
  - Clean Architecture style separation
  - Services, repositories, and CQRS patterns visible in the structure
  - Shared code across multiple applications

This solution follows a modular structure with multiple bounded contexts and clear separation of concerns between domain, application, and infrastructure layers.