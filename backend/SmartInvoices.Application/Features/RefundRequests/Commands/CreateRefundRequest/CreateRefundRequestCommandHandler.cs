using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Domain.Interfaces;
using SmartInvoices.Application.Interfaces;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Application.Features.RefundRequests.Commands.CreateRefundRequest;

/// <summary>
/// Handler komendy tworzenia nowego wniosku o zwrot.
/// </summary>
public class CreateRefundRequestCommandHandler
    : IRequestHandler<CreateRefundRequestCommand, Result<int>>
{
    private readonly IRefundRequestRepository _refundRequestRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<CreateRefundRequestCommandHandler> _logger;

    /// <summary>
    /// Konstruktor handlera CreateRefundRequestCommandHandler.
    /// </summary>
    /// <param name="refundRequestRepository">Repozytorium wniosków o zwrot</param>
    /// <param name="invoiceRepository">Repozytorium faktur</param>
    /// <param name="fileStorage">Serwis przechowywania plików</param>
    /// <param name="logger">Logger</param>
    public CreateRefundRequestCommandHandler(
        IRefundRequestRepository refundRequestRepository,
        IInvoiceRepository invoiceRepository,
        IFileStorage fileStorage,
        ILogger<CreateRefundRequestCommandHandler> logger
    )
    {
        _refundRequestRepository = refundRequestRepository;
        _invoiceRepository = invoiceRepository;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje komendę tworzenia nowego wniosku o zwrot.
    /// </summary>
    /// <param name="request">Komenda tworzenia wniosku</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>ID utworzonego wniosku</returns>
    public async Task<Result<int>> Handle(
        CreateRefundRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Creating new refund request for invoice ID: {InvoiceId}",
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

            // Tworzenie nowego wniosku o zwrot
            var refundRequest = new RefundRequest
            {
                InvoiceId = request.InvoiceId,
                Reason = request.Reason,
                Status = RefundRequestStatus.InProgress,
                RequestDate = DateTime.UtcNow,
                RefundRequestId = request.RequestedById,
                DocumentAttachments = new List<DocumentAttachment>()
            };

            // Przetwarzanie załączników
            foreach (var attachmentDto in request.Attachments)
            {
                // Zapisanie pliku w systemie plików
                var filePath = await _fileStorage.SaveFileAsync(
                    attachmentDto.FileName,
                    attachmentDto.ContentType,
                    attachmentDto.Content
                );

                // Utworzenie rekordu załącznika
                var attachment = new DocumentAttachment
                {
                    FileName = attachmentDto.FileName,
                    FileType = attachmentDto.ContentType,
                    FileSize = attachmentDto.Content.Length,
                    FilePath = filePath,
                    UploadDate = DateTime.UtcNow,
                    RefundRequest = refundRequest
                };

                refundRequest.DocumentAttachments.Add(attachment);
            }

            // Dodanie wniosku do repozytorium
            await _refundRequestRepository.AddAsync(refundRequest, cancellationToken);

            _logger.LogInformation(
                "Created new refund request with ID: {RefundRequestId}",
                refundRequest.RefundRequestId
            );

            return Result<int>.Success(refundRequest.RefundRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating refund request");
            return Result<int>.Failure("Wystąpił błąd podczas tworzenia wniosku o zwrot.");
        }
    }
}
