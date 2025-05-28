using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.Invoices.Queries;

/// <summary>
/// Handler zapytania o listę faktur.
/// </summary>
public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, InvoiceListDto>
{
    private readonly IInvoiceRepository _invoiceRepository;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    /// <summary>
    /// Obsługuje zapytanie o listę faktur.
    /// </summary>
    public async Task<InvoiceListDto> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var result = await _invoiceRepository.GetInvoicesAsync(
            userId: request.UserId,
            page: request.Page,
            pageSize: request.PageSize,
            status: request.Status,
            startDate: request.StartDate,
            endDate: request.EndDate,
            sortBy: request.SortBy,
            sortOrder: request.SortOrder,
            cancellationToken: cancellationToken);

        return result;
    }
}
