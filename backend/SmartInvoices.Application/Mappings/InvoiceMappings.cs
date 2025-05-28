using SmartInvoices.Application.DTOs;
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
            Id = invoice.InvoiceId,
            InvoiceNumber = invoice.InvoiceNumber,
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate,
            TotalAmount = invoice.TotalAmount,
            Status = invoice.Status.ToString(),
            Items =
                invoice.LineItems?.Select(item => item.ToDto()).ToList() ?? new List<LineItemDto>()
        };

    /// <summary>
    /// Mapuje listę obiektów Invoice na listę InvoiceDto.
    /// </summary>
    /// <param name="invoices">Lista faktur do zmapowania</param>
    /// <returns>Zmapowana lista InvoiceDto</returns>
    public static List<InvoiceDto> ToDto(this IEnumerable<Invoice> invoices) =>
        invoices?.Select(invoice => invoice.ToDto()).ToList() ?? new List<InvoiceDto>();
}
