using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Application.Interfaces.Repositories;
using SmartInvoices.Application.Mappings;

namespace SmartInvoices.Application.Features.Invoices.Queries.GetInvoiceDetail;

/// <summary>
/// Handler zapytania o szczegóły faktury.
/// </summary>
public class GetInvoiceDetailQueryHandler
    : IRequestHandler<GetInvoiceDetailQuery, Result<InvoiceDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<GetInvoiceDetailQueryHandler> _logger;

    /// <summary>
    /// Konstruktor handlera GetInvoiceDetailQueryHandler.
    /// </summary>
    /// <param name="invoiceRepository">Repozytorium faktur</param>
    /// <param name="logger">Logger</param>
    public GetInvoiceDetailQueryHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<GetInvoiceDetailQueryHandler> logger
    )
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje zapytanie o szczegóły faktury.
    /// </summary>
    /// <param name="request">Zapytanie o szczegóły faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>DTO faktury</returns>
    public async Task<Result<InvoiceDto>> Handle(
        GetInvoiceDetailQuery request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Getting invoice details for invoice ID: {InvoiceId}",
            request.InvoiceId
        );

        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(
                request.InvoiceId,
                cancellationToken
            );

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with ID {InvoiceId} not found", request.InvoiceId);
                return Result<InvoiceDto>.Failure(
                    $"Faktura o ID {request.InvoiceId} nie została znaleziona."
                );
            }

            var invoiceDto = invoice.ToDto();

            _logger.LogInformation(
                "Retrieved invoice details for invoice ID: {InvoiceId}",
                request.InvoiceId
            );

            return Result<InvoiceDto>.Success(invoiceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving invoice details for invoice ID: {InvoiceId}",
                request.InvoiceId
            );
            return Result<InvoiceDto>.Failure(
                "Wystąpił błąd podczas pobierania szczegółów faktury."
            );
        }
    }
}
