using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Interfaces.Repositories;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Features.Invoices.Commands.CreateInvoice;

/// <summary>
/// Handler komendy tworzenia nowej faktury.
/// </summary>
public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<int>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ILogger<CreateInvoiceCommandHandler> _logger;

    /// <summary>
    /// Konstruktor handlera CreateInvoiceCommandHandler.
    /// </summary>
    /// <param name="invoiceRepository">Repozytorium faktur</param>
    /// <param name="logger">Logger</param>
    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        ILogger<CreateInvoiceCommandHandler> logger
    )
    {
        _invoiceRepository = invoiceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje komendę tworzenia nowej faktury.
    /// </summary>
    /// <param name="request">Komenda tworzenia faktury</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>ID utworzonej faktury</returns>
    public async Task<Result<int>> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Creating new invoice with number: {InvoiceNumber}",
            request.InvoiceNumber
        );

        try
        {
            // Sprawdzenie, czy faktura o podanym numerze już istnieje
            var existingInvoice = await _invoiceRepository.GetByInvoiceNumberAsync(
                request.InvoiceNumber,
                cancellationToken
            );

            if (existingInvoice != null)
            {
                _logger.LogWarning(
                    "Invoice with number {InvoiceNumber} already exists",
                    request.InvoiceNumber
                );

                return Result<int>.Failure(
                    $"Faktura o numerze {request.InvoiceNumber} już istnieje."
                );
            }

            // Tworzenie nowej faktury
            var invoice = new Invoice
            {
                InvoiceNumber = request.InvoiceNumber,
                IssueDate = request.IssueDate,
                DueDate = request.DueDate,
                TotalAmount = request.TotalAmount,
                Status = Domain.Enums.InvoiceStatus.Draft,
                CreatedDate = DateTime.UtcNow,
                LineItems = request.Items
                    .Select(
                        item =>
                            new LineItem
                            {
                                Description = item.Description,
                                Quantity = (int)item.Quantity,
                                UnitPrice = item.UnitPrice,
                                TotalPrice = item.Quantity * item.UnitPrice
                            }
                    )
                    .ToList()
            };

            // Dodanie faktury do repozytorium
            await _invoiceRepository.AddAsync(invoice, cancellationToken);

            _logger.LogInformation("Created new invoice with ID: {InvoiceId}", invoice.InvoiceId);

            return Result<int>.Success(invoice.InvoiceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            return Result<int>.Failure("Wystąpił błąd podczas tworzenia faktury.");
        }
    }
}
