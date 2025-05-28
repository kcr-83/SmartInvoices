using SmartInvoices.Application.DTOs.LineItems;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Interfaces.Repositories;

/// <summary>
/// Interfejs repozytorium pozycji faktur.
/// </summary>
public interface ILineItemRepository
{
    /// <summary>
    /// Pobiera pozycję faktury po identyfikatorze.
    /// </summary>
    /// <param name="lineItemId">ID pozycji faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Pozycja faktury lub null, jeśli nie znaleziono</returns>
    Task<LineItem?> GetByIdAsync(int lineItemId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Pobiera fakturę na podstawie identyfikatora pozycji faktury.
    /// </summary>
    /// <param name="lineItemId">ID pozycji faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Faktura lub null, jeśli nie znaleziono</returns>
    Task<Invoice?> GetInvoiceByLineItemIdAsync(int lineItemId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Pobiera szczegóły pozycji faktury.
    /// </summary>
    /// <param name="lineItemId">ID pozycji faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Szczegóły pozycji faktury lub null, jeśli nie znaleziono</returns>
    Task<LineItemDto?> GetLineItemDtoByIdAsync(int lineItemId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Pobiera wszystkie pozycje dla danej faktury.
    /// </summary>
    /// <param name="invoiceId">ID faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Lista pozycji faktury</returns>
    Task<List<LineItemDto>> GetLineItemsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default);
}
