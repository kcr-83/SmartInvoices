using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;
using SmartInvoices.Application.Interfaces.Repositories;

namespace SmartInvoices.Application.Features.ChangeRequests.Queries;

/// <summary>
/// Handler zapytania o listę wniosków o zmiany.
/// </summary>
public class GetChangeRequestsQueryHandler : IRequestHandler<GetChangeRequestsQuery, ChangeRequestListDto>
{
    private readonly IChangeRequestRepository _changeRequestRepository;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetChangeRequestsQueryHandler(IChangeRequestRepository changeRequestRepository)
    {
        _changeRequestRepository = changeRequestRepository;
    }

    /// <summary>
    /// Obsługuje zapytanie o listę wniosków o zmiany.
    /// </summary>
    public async Task<ChangeRequestListDto> Handle(GetChangeRequestsQuery request, CancellationToken cancellationToken)
    {
        return await _changeRequestRepository.GetChangeRequestsAsync(
            request.UserId,
            request.Page,
            request.PageSize,
            request.Status,
            cancellationToken);
    }
}
