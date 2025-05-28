using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Domain.Interfaces;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands.CreateChangeRequest;

/// <summary>
/// Handler komendy tworzenia nowego wniosku o zmianę.
/// </summary>
public class CreateChangeRequestCommandHandler
    : IRequestHandler<CreateChangeRequestCommand, Result<int>>
{
    private readonly IChangeRequestRepository _changeRequestRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<CreateChangeRequestCommandHandler> _logger;

    /// <summary>
    /// Konstruktor handlera CreateChangeRequestCommandHandler.
    /// </summary>
    /// <param name="changeRequestRepository">Repozytorium wniosków o zmianę</param>
    /// <param name="invoiceRepository">Repozytorium faktur</param>
    /// <param name="logger">Logger</param>
    public CreateChangeRequestCommandHandler(
        IChangeRequestRepository changeRequestRepository,
        IInvoiceRepository invoiceRepository,
        ILogger<CreateChangeRequestCommandHandler> logger
    )
    {
        _changeRequestRepository = changeRequestRepository;
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje komendę tworzenia nowego wniosku o zmianę.
    /// </summary>
    /// <param name="request">Komenda tworzenia wniosku</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>ID utworzonego wniosku</returns>
    public async Task<Result<int>> Handle(
        CreateChangeRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Creating new change request for invoice ID: {InvoiceId}",
            request.InvoiceId
        );

        try
        {
            // Sprawdzenie, czy faktura istnieje
            var invoice = await _invoiceRepository.GetByIdAsync(
                request.InvoiceId,
                cancellationToken
            );
            if (invoice == null)
            {
                _logger.LogWarning("Invoice with ID {InvoiceId} not found", request.InvoiceId);
                return Result<int>.Failure(
                    $"Faktura o ID {request.InvoiceId} nie została znaleziona."
                );
            }

            // Sprawdzenie, czy wszystkie pozycje faktury istnieją
            var allLineItemsExist = request.LineItemIds.All(
                id => invoice.LineItems.Any(item => item.LineItemId == id)
            );
            if (!allLineItemsExist)
            {
                _logger.LogWarning("Some line items do not exist in the invoice");
                return Result<int>.Failure("Niektóre pozycje faktury nie istnieją.");
            }

            // Tworzenie nowego wniosku o zmianę
            var changeRequest = new ChangeRequest
            {
                InvoiceId = request.InvoiceId,
                LineItemIds = request.LineItemIds,
                Reason = request.Justification,
                Status = RequestStatus.InProgress,
                RequestDate = DateTime.UtcNow,
                ChangeRequestId = request.RequestedById
            };

            // Dodanie wniosku do repozytorium
            await _changeRequestRepository.AddAsync(changeRequest, cancellationToken);

            _logger.LogInformation(
                "Created new change request with ID: {ChangeRequestId}",
                changeRequest.ChangeRequestId
            );

            return Result<int>.Success(changeRequest.ChangeRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating change request");
            return Result<int>.Failure("Wystąpił błąd podczas tworzenia wniosku o zmianę.");
        }
    }
}
