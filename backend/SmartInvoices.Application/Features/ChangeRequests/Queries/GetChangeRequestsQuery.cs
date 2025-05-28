using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;

namespace SmartInvoices.Application.Features.ChangeRequests.Queries;

/// <summary>
/// Zapytanie o listę wniosków o zmiany.
/// </summary>
public class GetChangeRequestsQuery : IRequest<ChangeRequestListDto>
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Numer strony.
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Rozmiar strony.
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Status wniosku do filtrowania.
    /// </summary>
    public string? Status { get; set; }
}
