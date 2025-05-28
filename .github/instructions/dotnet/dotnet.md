## General Guidelines for .NET Development
- Use the latest stable version of .NET for new projects to leverage performance improvements and new features
- Follow the SOLID principles to ensure maintainable and scalable code
- Use asynchronous programming patterns (async/await) for I/O-bound operations to improve responsiveness
- Use dependency injection to manage service lifetimes and promote loose coupling
- Implement logging using a structured logging framework like Serilog for better traceability
- Use configuration files (appsettings.json) for managing application settings and secrets
- Use XML comments for public APIs to provide clear documentation and improve code readability
- Use consistent naming conventions for classes, methods, and variables to enhance code clarity
- Use code analysis tools like Roslyn analyzers to enforce coding standards and detect potential issues early
- Use unit tests to validate business logic and ensure code quality; prefer xUnit for testing frameworks  
- Use integration tests to validate the interaction between components and external systems
- Use code reviews to ensure adherence to coding standards and improve code quality
- use centralized project management for .NET projects to ensure consistency and maintainability across multiple repositories
- use `dotnet-central-management.md` file where is described centralized .NET project management.


### Guidelines for DOTNET

// GitHub Copilot: Always use file-scoped namespaces (C# 10+ style)
// Example: namespace MyProject.Services;
// NOT: namespace MyProject.Services { ... }

namespace StyleGuideExample;

public class Example
{
    // Class implementation
}

#### ENTITY_FRAMEWORK

- Use the repository and unit of work patterns to abstract data access logic and simplify testing
- Implement eager loading with Include() to avoid N+1 query problems for {{entity_relationships}}
- Use migrations for database schema changes and version control with proper naming conventions
- Apply appropriate tracking behavior (AsNoTracking() for read-only queries) to optimize performance
- Implement query optimization techniques like compiled queries for frequently executed database operations
- Use value conversions for complex property transformations and proper handling of custom types
- Implement proper error handling and logging for database operations to ensure reliability and traceability

#### ASP_NET
- Use minimal APIs for simple endpoints in .NET 6+ applications to reduce boilerplate code; for the minimal API approach, use the FastEndpoints library
- Implement the mediator pattern without MediatR for decoupling request handling and simplifying cross-cutting concerns, becouse is paid, use own implementation
- Use FluentValidation for model validation to ensure clean and maintainable validation logic
- Use API controllers with model binding and validation attributes for structured request handling
- Implement proper versioning strategies for APIs, such as URL versioning or header versioning
- Apply proper response caching with cache profiles and ETags for improved performance on static resources
- Implement proper exception handling with ExceptionFilter or middleware to provide consistent error responses
- Use dependency injection with scoped lifetime for request-specific services and singleton for stateless services