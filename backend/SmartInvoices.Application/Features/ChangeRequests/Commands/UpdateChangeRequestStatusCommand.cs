using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands;

/// <summary>
/// Komenda aktualizacji statusu wniosku o zmianę.
/// </summary>
public class UpdateChangeRequestStatusCommand : IRequest<ChangeRequestDetailDto?>
{
    /// <summary>
    /// Identyfikator wniosku o zmianę.
    /// </summary>
    public int ChangeRequestId { get; set; }
    
    /// <summary>
    /// Nowy status wniosku.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Notatki administratora.
    /// </summary>
    public string? AdminNotes { get; set; }
    
    /// <summary>
    /// Identyfikator administratora.
    /// </summary>
    public int AdminId { get; set; }
}
