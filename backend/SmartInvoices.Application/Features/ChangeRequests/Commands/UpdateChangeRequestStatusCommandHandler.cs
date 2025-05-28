using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands;

/// <summary>
/// Handler komendy aktualizacji statusu wniosku o zmianę.
/// </summary>
public class UpdateChangeRequestStatusCommandHandler : IRequestHandler<UpdateChangeRequestStatusCommand, ChangeRequestDetailDto?>
{
    private readonly IChangeRequestRepository _changeRequestRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public UpdateChangeRequestStatusCommandHandler(
        IChangeRequestRepository changeRequestRepository,
        IUserRepository userRepository)
    {
        _changeRequestRepository = changeRequestRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Obsługuje komendę aktualizacji statusu wniosku o zmianę.
    /// </summary>
    public async Task<ChangeRequestDetailDto?> Handle(UpdateChangeRequestStatusCommand request, CancellationToken cancellationToken)
    {
        // Sprawdzenie czy użytkownik ma uprawnienia administratora
        var admin = await _userRepository.GetByIdAsync(request.AdminId, cancellationToken);
        if (admin == null || !admin.IsAdmin)
        {
            throw new UnauthorizedAccessException("Tylko administrator może aktualizować status wniosku");
        }

        // Walidacja statusu
        if (string.IsNullOrEmpty(request.Status) || 
            !new[] { "Approved", "Rejected", "InProgress" }.Contains(request.Status))
        {
            throw new ArgumentException("Nieprawidłowy status wniosku");
        }

        // Aktualizacja statusu
        var updateDto = new UpdateChangeRequestStatusDto
        {
            ChangeRequestId = request.ChangeRequestId,
            Status = request.Status,
            AdminNotes = request.AdminNotes,
            AdminId = request.AdminId
        };

        var updatedRequest = await _changeRequestRepository.UpdateStatusAsync(updateDto, cancellationToken);
        if (updatedRequest == null)
        {
            return null;
        }

        // Pobranie zaktualizowanego wniosku
        return await _changeRequestRepository.GetChangeRequestByIdAsync(
            request.ChangeRequestId, 
            request.AdminId, 
            cancellationToken);
    }
}
