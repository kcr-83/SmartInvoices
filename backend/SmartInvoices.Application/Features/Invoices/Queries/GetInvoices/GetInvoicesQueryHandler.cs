using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs;
using SmartInvoices.Application.Mappings;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Application.Features.Invoices.Queries.GetInvoices;

/// <summary>
/// Handler zapytania o listę faktur.
/// </summary>
public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, Result<List<InvoiceDto>>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<GetInvoicesQueryHandler> _logger;

    /// <summary>
    /// Konstruktor handlera GetInvoicesQueryHandler.
    /// </summary>
    /// <param name="invoiceRepository">Repozytorium faktur</param>
    /// <param name="logger">Logger</param>
    public GetInvoicesQueryHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<GetInvoicesQueryHandler> logger
    )
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje zapytanie o listę faktur.
    /// </summary>
    /// <param name="request">Zapytanie o faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Lista DTO faktur</returns>
    public async Task<Result<List<InvoiceDto>>> Handle(
        GetInvoicesQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Getting invoices with search criteria: {SearchString}, from {StartDate} to {EndDate}",
            request.SearchString,
            request.StartDate,
            request.EndDate
        );

        try
        {
            var invoices = await _invoiceRepository.GetInvoicesAsync(
                request.SearchString,
                request.StartDate,
                request.EndDate,
                request.Status,
                request.MinAmount,
                request.MaxAmount,
                request.SortBy,
                request.SortDirection,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            var invoiceDtos = invoices.ToDto();

            _logger.LogInformation("Retrieved {Count} invoices", invoiceDtos.Count);

            return Result<List<InvoiceDto>>.Success(invoiceDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices");
            return Result<List<InvoiceDto>>.Failure("Wystąpił błąd podczas pobierania faktur.");
        }
    }
}
