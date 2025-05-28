using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands;

/// <summary>
/// Handler komendy tworzenia wniosku o zmianę.
/// </summary>
public class CreateChangeRequestCommandHandler
    : IRequestHandler<CreateChangeRequestCommand, ChangeRequestDetailDto>
{
    private readonly IChangeRequestRepository _changeRequestRepository;
    private readonly ILineItemRepository _lineItemRepository;

    /// <summary>
    /// Konstruktor.
    /// /// </summary>
    public CreateChangeRequestCommandHandler(
        IChangeRequestRepository changeRequestRepository,
        ILineItemRepository lineItemRepository
    )
    {
        _changeRequestRepository = changeRequestRepository;
        _lineItemRepository = lineItemRepository;
    }

    /// <summary>
    /// Obsługuje komendę tworzenia wniosku o zmianę.
    /// </summary>
    public async Task<ChangeRequestDetailDto> Handle(
        CreateChangeRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        // Sprawdzenie czy pozycja faktury istnieje
        var lineItem = await _lineItemRepository.GetByIdAsync(
            request.LineItemId,
            cancellationToken
        );
        if (lineItem == null)
        {
            throw new KeyNotFoundException(
                $"Pozycja faktury o ID {request.LineItemId} nie istnieje"
            );
        }

        // Sprawdzenie czy faktura może być zmieniona (nie jest np. opłacona)
        var invoice = await _lineItemRepository.GetInvoiceByLineItemIdAsync(
            request.LineItemId,
            cancellationToken
        );
        if (invoice == null)
        {
            throw new InvalidOperationException("Faktura nie może być zmodyfikowana");
        }

        var createDto = new CreateChangeRequestDto
        {
            LineItemId = request.LineItemId,
            RequestedQuantity = request.RequestedQuantity,
            RequestedUnitPrice = request.RequestedUnitPrice,
            RequestedDescription = request.RequestedDescription,
            Reason = request.Reason,
            UserId = request.UserId
        };

        // Utworzenie wniosku
        var changeRequest = await _changeRequestRepository.CreateChangeRequestAsync(
            createDto,
            cancellationToken
        );

        // Pobranie szczegółów utworzonego wniosku
        var result = await _changeRequestRepository.GetChangeRequestByIdAsync(
            changeRequest.ChangeRequestId,
            request.UserId,
            cancellationToken
        );

        if (result == null)
        {
            throw new Exception("Wystąpił błąd podczas pobierania utworzonego wniosku o zmianę");
        }

        return result;
    }
}
