using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.ChangeRequests.Queries;

/// <summary>
/// Handler zapytania o szczegóły wniosku o zmianę.
/// </summary>
public class GetChangeRequestByIdQueryHandler : IRequestHandler<GetChangeRequestByIdQuery, ChangeRequestDetailDto?>
{
    private readonly IChangeRequestRepository _changeRequestRepository;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetChangeRequestByIdQueryHandler(IChangeRequestRepository changeRequestRepository)
    {
        _changeRequestRepository = changeRequestRepository;
    }

    /// <summary>
    /// Obsługuje zapytanie o szczegóły wniosku o zmianę.
    /// </summary>
    public async Task<ChangeRequestDetailDto?> Handle(GetChangeRequestByIdQuery request, CancellationToken cancellationToken)
    {
        return await _changeRequestRepository.GetChangeRequestByIdAsync(
            request.ChangeRequestId,
            request.UserId,
            cancellationToken);
    }
}
