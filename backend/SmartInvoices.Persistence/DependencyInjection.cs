using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartInvoices.Domain.Interfaces;
using SmartInvoices.Persistence.Repositories;
using Microsoft.Extensions.Configuration;

namespace SmartInvoices.Persistence
{
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
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Rejestracja kontekstu bazy danych jako InMemoryDb
            services.AddDbContext<AppDbContext>(
                options => options.UseInMemoryDatabase("SmartInvoicesDb")
            );

            // Rejestracja IDbContext
            services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            // Rejestracja repozytoriów
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILineItemRepository, LineItemRepository>();
            services.AddScoped<IRefundRequestRepository, RefundRequestRepository>();

            return services;
        }
    }
}
