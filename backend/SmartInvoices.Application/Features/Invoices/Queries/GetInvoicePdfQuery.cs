using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Features.Invoices.Queries;

/// <summary>
/// Zapytanie o fakturę w formacie PDF.
/// </summary>
public class GetInvoicePdfQuery : IRequest<byte[]?>
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
