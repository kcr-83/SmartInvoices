using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;

namespace SmartInvoices.Application.Features.ChangeRequests.Queries;

/// <summary>
/// Zapytanie o szczegóły wniosku o zmianę.
/// </summary>
public class GetChangeRequestByIdQuery : IRequest<ChangeRequestDetailDto?>
{
    /// <summary>
    /// Identyfikator wniosku o zmianę.
    /// </summary>
    public int ChangeRequestId { get; set; }
    
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public int UserId { get; set; }
}
