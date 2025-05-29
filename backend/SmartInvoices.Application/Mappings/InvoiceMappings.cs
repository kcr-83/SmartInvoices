using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Application.DTOs.LineItems;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Mappings;

/// <summary>
/// Klasa pomocnicza do mapowania obiektów Invoice na InvoiceDto i odwrotnie.
/// </summary>
public static class InvoiceMappings
{
    /// <summary>
    /// Mapuje obiekt Invoice na InvoiceDto.
    /// </summary>
    /// <param name="invoice">Faktura do zmapowania</param>
    /// <returns>Zmapowany obiekt InvoiceDto</returns>
    public static InvoiceDto ToDto(this Invoice invoice) =>
        new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            TotalAmount = invoice.TotalAmount,
            TaxAmount = invoice.TaxAmount,
            Status = invoice.Status.ToString(),
            PaymentDate = invoice.PaymentDate
        };

    /// <summary>
    /// Mapuje obiekt Invoice na InvoiceDetailDto.
    /// </summary>
    /// <param name="invoice">Faktura do zmapowania</param>
    /// <returns>Zmapowany obiekt InvoiceDetailDto</returns>
    public static InvoiceDetailDto ToDetailDto(this Invoice invoice) =>
        new InvoiceDetailDto
        {
            InvoiceId = invoice.InvoiceId,
            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            TotalAmount = invoice.TotalAmount,
            TaxAmount = invoice.TaxAmount,
            Status = invoice.Status.ToString(),
            PaymentDate = invoice.PaymentDate,
            Notes = invoice.Notes,
            LineItems =
                invoice.LineItems?.Select(item => item.ToDto()).ToList()
                ??
                new List<LineItemDto>()
        };

    /// <summary>
    /// Mapuje listę obiektów Invoice na listę InvoiceDto.
    /// </summary>
    /// <param name="invoices">Lista faktur do zmapowania</param>
    /// <returns>Zmapowana lista InvoiceDto</returns>
    public static List<InvoiceDto> ToDto(this IEnumerable<Invoice> invoices) =>
        invoices?.Select(invoice => invoice.ToDto()).ToList() ?? new List<InvoiceDto>();
}
