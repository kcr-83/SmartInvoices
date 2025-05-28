namespace SmartInvoices.Application.DTOs.ChangeRequests;

/// <summary>
/// DTO dla tworzenia wniosku o zmianę.
/// </summary>
public class CreateChangeRequestDto
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
