using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.Invoices.Queries;

/// <summary>
/// Handler zapytania o fakturę w formacie PDF.
/// </summary>
public class GetInvoicePdfQueryHandler : IRequestHandler<GetInvoicePdfQuery, byte[]?>
{
    private readonly IInvoiceRepository _invoiceRepository;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetInvoicePdfQueryHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    /// <summary>
    /// Obsługuje zapytanie o fakturę w formacie PDF.
    /// </summary>
    public async Task<byte[]?> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken)
    {
        return await _invoiceRepository.GeneratePdfAsync(request.InvoiceId, request.UserId, cancellationToken);
    }
}
