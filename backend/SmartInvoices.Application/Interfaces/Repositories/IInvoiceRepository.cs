using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Application.Interfaces.Repositories;

/// <summary>
/// Interfejs repozytorium faktur.
/// </summary>
public interface IInvoiceRepository
{
    /// <summary>
    /// Pobiera paginowaną i filtrowaną listę faktur.
    /// </summary>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="page">Numer strony</param>
    /// <param name="pageSize">Rozmiar strony</param>
    /// <param name="status">Status faktury (opcjonalnie)</param>
    /// <param name="startDate">Data początkowa (opcjonalnie)</param>
    /// <param name="endDate">Data końcowa (opcjonalnie)</param>
    /// <param name="sortBy">Pole do sortowania (opcjonalnie)</param>
    /// <param name="sortOrder">Kierunek sortowania (asc/desc) (opcjonalnie)</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Paginowana lista faktur</returns>
    Task<InvoiceListDto> GetInvoicesAsync(
        int userId,
        int page = 1,
        int pageSize = 10,
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? sortBy = null,
        string? sortOrder = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera szczegóły faktury po identyfikatorze.
    /// </summary>
    /// <param name="invoiceId">ID faktury</param>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Szczegóły faktury lub null, jeśli nie znaleziono</returns>
    Task<InvoiceDetailDto?> GetInvoiceByIdAsync(
        int invoiceId,
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera fakturę po identyfikatorze.
    /// </summary>
    /// <param name="invoiceId">ID faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Faktura lub null, jeśli nie znaleziono</returns>
    Task<Invoice?> GetByIdAsync(
        int invoiceId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generuje fakturę w formacie PDF.
    /// </summary>
    /// <param name="invoiceId">ID faktury</param>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Dane PDF lub null, jeśli nie znaleziono</returns>
    Task<byte[]?> GeneratePdfAsync(
        int invoiceId,
        int userId,
        CancellationToken cancellationToken = default);
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
