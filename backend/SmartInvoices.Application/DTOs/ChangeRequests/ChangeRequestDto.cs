namespace SmartInvoices.Application.DTOs.ChangeRequests;

/// <summary>
/// DTO dla wniosku o zmianę pozycji faktury.
/// </summary>
public class ChangeRequestDto
{
    /// <summary>
    /// Identyfikator wniosku o zmianę.
    /// </summary>
    public int ChangeRequestId { get; set; }
    
    /// <summary>
    /// Status wniosku.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Data złożenia wniosku.
    /// </summary>
    public DateTime RequestDate { get; set; }
    
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
}
