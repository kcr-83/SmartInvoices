using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Invoices;

namespace SmartInvoices.Application.Features.Invoices.Queries;

/// <summary>
/// Zapytanie o szczegóły faktury po identyfikatorze.
/// </summary>
public class GetInvoiceByIdQuery : IRequest<InvoiceDetailDto?>
{
    /// <summary>
    /// Identyfikator faktury.
    /// </summary>
    public int InvoiceId { get; set; }
    
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public int UserId { get; set; }
}
