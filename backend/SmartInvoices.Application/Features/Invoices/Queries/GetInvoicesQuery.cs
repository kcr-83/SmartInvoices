using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Invoices;

namespace SmartInvoices.Application.Features.Invoices.Queries;

/// <summary>
/// Zapytanie o listę faktur z paginacją i filtrowaniem.
/// </summary>
public class GetInvoicesQuery : IRequest<InvoiceListDto>
{
    /// <summary>
    /// Numer strony.
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Rozmiar strony.
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Status faktury do filtrowania.
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Data początkowa do filtrowania.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Data końcowa do filtrowania.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Pole do sortowania.
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Kierunek sortowania (asc/desc).
    /// </summary>
    public string? SortOrder { get; set; }
    
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public int UserId { get; set; }
}
