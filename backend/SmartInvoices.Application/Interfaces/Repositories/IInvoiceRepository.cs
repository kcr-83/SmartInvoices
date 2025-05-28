using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Domain.Entities;

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
}
