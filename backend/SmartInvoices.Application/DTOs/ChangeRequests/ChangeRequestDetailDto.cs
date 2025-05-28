using SmartInvoices.Application.DTOs.LineItems;

namespace SmartInvoices.Application.DTOs.ChangeRequests;

/// <summary>
/// DTO dla szczegółów wniosku o zmianę.
/// </summary>
public class ChangeRequestDetailDto
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
    /// Identyfikator faktury.
    /// </summary>
    public int InvoiceId { get; set; }
    
    /// <summary>
    /// Numer faktury.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Pozycja faktury, której dotyczy wniosek.
    /// </summary>
    public LineItemDto LineItem { get; set; } = null!;
    
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
    /// Notatki administratora dotyczące wniosku.
    /// </summary>
    public string? AdminNotes { get; set; }
    
    /// <summary>
    /// Identyfikator użytkownika, który rozpatrzył wniosek.
    /// </summary>
    public int? ReviewedBy { get; set; }
    
    /// <summary>
    /// Data rozpatrzenia wniosku.
    /// </summary>
    public DateTime? ReviewDate { get; set; }
}
