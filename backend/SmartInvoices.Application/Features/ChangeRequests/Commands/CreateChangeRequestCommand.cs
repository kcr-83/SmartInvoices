using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs.ChangeRequests;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands;

/// <summary>
/// Komenda tworzenia wniosku o zmianę.
/// </summary>
public class CreateChangeRequestCommand : IRequest<ChangeRequestDetailDto>
{
    /// <summary>
    /// Identyfikator pozycji faktury.
    /// </summary>
    public int LineItemId { get; set; }
    
    /// <summary>
    /// Wnioskowana ilość.
    /// </summary>
    public int? RequestedQuantity { get; set; }
    
    /// <summary>
    /// Wnioskowana cena jednostkowa.
    /// </summary>
    public decimal? RequestedUnitPrice { get; set; }
    
    /// <summary>
    /// Wnioskowany opis pozycji.
    /// </summary>
    public string? RequestedDescription { get; set; }
    
    /// <summary>
    /// Powód wniosku o zmianę.
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    
    /// <summary>
    /// Identyfikator użytkownika składającego wniosek.
    /// </summary>
    public int UserId { get; set; }
}
