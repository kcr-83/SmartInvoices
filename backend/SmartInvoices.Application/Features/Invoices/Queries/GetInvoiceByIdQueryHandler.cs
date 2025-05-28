using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.Invoices.Queries;

/// <summary>
/// Handler zapytania o szczegóły faktury.
/// </summary>
public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailDto?>
{
    private readonly IInvoiceRepository _invoiceRepository;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetInvoiceByIdQueryHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    /// <summary>
    /// Obsługuje zapytanie o szczegóły faktury.
    /// </summary>
    public async Task<InvoiceDetailDto?> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        return await _invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId, request.UserId, cancellationToken);
    }
}
