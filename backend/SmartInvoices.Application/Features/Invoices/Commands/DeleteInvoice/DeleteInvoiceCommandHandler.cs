using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.Invoices.Commands.DeleteInvoice;

/// <summary>
/// Handler komendy usuwania faktury.
/// </summary>
public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<DeleteInvoiceCommandHandler> _logger;

    /// <summary>
    /// Konstruktor handlera DeleteInvoiceCommandHandler.
    /// </summary>
    /// <param name="invoiceRepository">Repozytorium faktur</param>
    /// <param name="logger">Logger</param>
    public DeleteInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<DeleteInvoiceCommandHandler> logger
    )
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje komendę usuwania faktury.
    /// </summary>
    /// <param name="request">Komenda usuwania faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Wynik operacji</returns>
    public async Task<Result> Handle(
        DeleteInvoiceCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Deleting invoice with ID: {InvoiceId}", request.InvoiceId);

        try
        {
            // Pobranie faktury do usunięcia
            var invoice = await _invoiceRepository.GetByIdAsync(
                request.InvoiceId,
                cancellationToken
            );

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with ID {InvoiceId} not found", request.InvoiceId);
                return Result.Failure($"Faktura o ID {request.InvoiceId} nie została znaleziona.");
            }

            // Sprawdzenie, czy faktura może być usunięta (np. czy nie ma powiązanych wniosków)
            // Tutaj można dodać dodatkową logikę biznesową sprawdzającą, czy faktura może być usunięta

            // Usunięcie faktury
            await _invoiceRepository.DeleteAsync(invoice, cancellationToken);

            _logger.LogInformation("Deleted invoice with ID: {InvoiceId}", request.InvoiceId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice with ID: {InvoiceId}", request.InvoiceId);
            return Result.Failure("Wystąpił błąd podczas usuwania faktury.");
        }
    }
}
