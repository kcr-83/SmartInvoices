using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Features.Invoices.Commands.DeleteInvoice;

/// <summary>
/// Komenda do usunięcia faktury.
/// </summary>
public class DeleteInvoiceCommand : IRequest<Result>
{
    /// <summary>
    /// Identyfikator faktury do usunięcia.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Konstruktor komendy DeleteInvoiceCommand.
    /// </summary>
    /// <param name="invoiceId">Identyfikator faktury do usunięcia</param>
    public DeleteInvoiceCommand(int invoiceId)
    {
        InvoiceId = invoiceId;
    }
}
