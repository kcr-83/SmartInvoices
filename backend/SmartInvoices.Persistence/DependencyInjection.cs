using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoices.Domain.Interfaces;
using SmartInvoices.Persistence.Repositories;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Persistence;

/// <summary>
/// Klasa konfiguracyjna dla warstwy persystencji
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Metoda rozszerzająca do konfiguracji serwisów persystencji w kontenerze DI
    /// </summary>
    /// <param name="services">Kolekcja serwisów</param>
    /// <returns>Zaktualizowana kolekcja serwisów</returns>
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Konfiguracja bazy danych
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

        try
        {
            SeedTestData(services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas inicjalizacji bazy danych: {ex.Message}");
        }
        // Rejestracja repozytoriów
        services.AddScoped<IDbContext, AppDbContext>();
        services.AddScoped<Application.Interfaces.Repositories.IUserRepository, UserRepository>();
        //services.AddScoped<ILocalFileStorage, LocalFileStorage>();
        services.AddScoped<
            SmartInvoices.Application.Interfaces.Repositories.IInvoiceRepository,
            InvoiceRepository
        >();
        services.AddScoped<
            SmartInvoices.Application.Interfaces.Repositories.IChangeRequestRepository,
            ChangeRequestRepository
        >();

        services.AddScoped<IRefundRequestRepository, RefundRequestRepository>();
        services.AddScoped<SmartInvoices.Application.Interfaces.Repositories.ILineItemRepository, LineItemRepository>();

        return services;
    }

    /// <summary>
    /// Metoda rozszerzająca do inicjalizacji bazy danych danymi testowymi
    /// </summary>
    /// <param name="app">Aplikacja</param>
    /// <returns>Aplikacja</returns>
    public static void SeedTestData(this IServiceCollection services)
    {
        var dbContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
        SeedData(dbContext);
    }

    /// <summary>
    /// Metoda inicjalizująca bazę danych testowymi danymi
    /// </summary>
    /// <param name="context">Kontekst bazy danych</param>
    private static void SeedData(AppDbContext context)
    {
        // Sprawdzenie czy baza już zawiera dane
        if (context.Users.Any())
        {
            return; // Dane już istnieją, więc nie dodajemy ich ponownie
        }

        // Dodanie użytkowników testowych
        var users = new List<User>
        {
            new User
            {
                UserId = 1,
                FirstName = "admin",
                LastName = "adminowski",
                Email = "admin@example.com",
                PasswordHash = "Admin123!",
                IsAdmin = true,
            },
            new User
            {
                UserId = 2,
                Email = "user1@example.com",
                PasswordHash = "User123!",
                FirstName = "Jan",
                LastName = "Kowalski",
                IsAdmin = false,
            },
            new User
            {
                UserId = 3,
                Email = "user2@example.com",
                PasswordHash = "User123!",
                FirstName = "Anna",
                LastName = "Nowak",
                IsAdmin = false
            }
        };
        context.Users.AddRange(users);

        // Dodanie przykładowych faktur
        var invoices = new List<Invoice>
        {
            new Invoice
            {
                InvoiceId = 1,
                InvoiceNumber = "FV/2023/001",
                IssueDate = DateTime.Now.AddDays(-30),
                DueDate = DateTime.Now.AddDays(15),
                UserId = 2,
                CreatedDate = DateTime.Now.AddDays(-30),
                Notes = "Faktura za usługi programistyczne",
                Status = Domain.Enums.InvoiceStatus.Paid,
                PaymentDate = DateTime.Now.AddDays(-10),
                TaxAmount = 500.00m,
                TotalAmount = 6000.00m,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        LineItemId = 1,
                        Description = "Usługa programistyczna",
                        Quantity = 1,
                        UnitPrice = 6000.00m,
                        TotalPrice = 6000.00m
                    }
                }
            },
            new Invoice
            {
                InvoiceId = 2,
                InvoiceNumber = "FV/2023/002",
                IssueDate = DateTime.Now.AddDays(-20),
                DueDate = DateTime.Now.AddDays(10),
                UserId = 2,
                CreatedDate = DateTime.Now.AddDays(-30),
                Notes = "Faktura za usługi doradcze",
                Status = Domain.Enums.InvoiceStatus.Paid,
                PaymentDate = DateTime.Now.AddDays(-15),
                TaxAmount = 400.00m,
                TotalAmount = 8000.00m,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        LineItemId = 1,
                        Description = "Usługa doradcza",
                        Quantity = 1,
                        UnitPrice = 8000.00m,
                        TotalPrice = 8000.00m
                    }
                }
            },
            new Invoice
            {
                InvoiceId = 3,
                InvoiceNumber = "FV/2023/003",
                IssueDate = DateTime.Now.AddDays(-10),
                DueDate = DateTime.Now.AddDays(20),
                UserId = 3,
                CreatedDate = DateTime.Now.AddDays(-30),
                Notes = "Faktura za usługi graficzne",
                Status = Domain.Enums.InvoiceStatus.Paid,
                PaymentDate = DateTime.Now.AddDays(-15),
                TaxAmount = 250.00m,
                TotalAmount = 3075.00m,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        LineItemId = 1,
                        Description = "Usługa graficzna",
                        Quantity = 1,
                        UnitPrice = 3075.00m,
                        TotalPrice = 3075.00m
                    }
                }
            }
        };
        context.Invoices.AddRange(invoices);

        // Dodanie przykładowych wniosków o zmianę
        var changeRequests = new List<ChangeRequest>
        {
            new ChangeRequest
            {
                ChangeRequestId = 1,
                InvoiceId = 1,
                UserId = 2,
                RequestDate = DateTime.Now.AddDays(-5),
                Status = Domain.Enums.RequestStatus.InProgress,
                Reason = "Proszę o korektę nazwy klienta",
                RequestedDescription =
                    "Zmiana nazwy klienta na 'Firma ABC International Sp. z o.o.'"
            },
            new ChangeRequest
            {
                ChangeRequestId = 2,
                InvoiceId = 3,
                UserId = 3,
                RequestDate = DateTime.Now.AddDays(-2),
                Status = Domain.Enums.RequestStatus.Approved,
                Reason = "Proszę o zmianę terminu płatności",
                RequestedDescription = "Wydłużenie terminu płatności o 14 dni"
            }
        };
        context.ChangeRequests.AddRange(changeRequests);

        context.SaveChanges();
    }
}
