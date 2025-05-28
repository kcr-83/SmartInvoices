using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Interfaces
{
    /// <summary>
    /// Interfejs repozytorium do zarządzania fakturami
    /// </summary>
    public interface IInvoiceRepository
    {
        /// <summary>
        /// Pobiera wszystkie faktury
        /// </summary>
        Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Pobiera faktury danego użytkownika
        /// </summary>
        Task<IEnumerable<Invoice>> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera faktury wg. kryteriów filtrowania
        /// </summary>
        Task<IEnumerable<Invoice>> GetFilteredAsync(
            int? userId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            InvoiceStatus? status = null,
            CancellationToken cancellationToken = default
        );
        /// <summary>
        /// Pobiera faktury wg. kryteriów wyszukiwania
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <param name="minAmount"></param>
        /// <param name="maxAmount"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Invoice>> GetInvoicesAsync(
            string? searchString,
            DateTime? startDate,
            DateTime? endDate,
            string? status,
            decimal? minAmount,
            decimal? maxAmount,
            string? sortBy,
            string? sortDirection,
            int? pageNumber,
            int? pageSize,
            CancellationToken cancellationToken = default
        );
        /// <summary>
        /// Pobiera fakturę wg. identyfikatora
        /// </summary>
        Task<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pobiera fakturę wg. numeru faktury
        /// </summary>
        Task<Invoice> GetByInvoiceNumberAsync(
            string invoiceNumber,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Dodaje nową fakturę
        /// </summary>
        Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aktualizuje istniejącą fakturę
        /// </summary>
        Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Usuwa fakturę
        /// </summary>
        Task DeleteAsync(Invoice invoice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pobiera pozycję faktury wg. identyfikatora
        /// </summary>
        Task<LineItem> GetLineItemByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pobiera pozycje danej faktury
        /// </summary>
        Task<IEnumerable<LineItem>> GetLineItemsByInvoiceIdAsync(
            int invoiceId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Dodaje pozycję faktury
        /// </summary>
        Task AddLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aktualizuje pozycję faktury
        /// </summary>
        Task UpdateLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default);

        /// <summary>
        /// Usuwa pozycję faktury
        /// </summary>
        Task DeleteLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default);
    }
}
