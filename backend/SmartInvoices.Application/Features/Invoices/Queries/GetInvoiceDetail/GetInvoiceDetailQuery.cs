using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Invoices;

namespace SmartInvoices.Application.Features.Invoices.Queries.GetInvoiceDetail;

/// <summary>
/// Zapytanie zwracające szczegóły konkretnej faktury.
/// </summary>
public class GetInvoiceDetailQuery : IRequest<Result<InvoiceDto>>
{
    /// <summary>
    /// Identyfikator faktury.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Konstruktor zapytania GetInvoiceDetailQuery.
    /// </summary>
    /// <param name="invoiceId">Identyfikator faktury</param>
    public GetInvoiceDetailQuery(int invoiceId)
    {
        InvoiceId = invoiceId;
    }
}
