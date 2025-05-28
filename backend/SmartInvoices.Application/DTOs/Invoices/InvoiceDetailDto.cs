using SmartInvoices.Application.DTOs.LineItems;

namespace SmartInvoices.Application.DTOs.Invoices;

/// <summary>
/// DTO dla szczegółowych informacji o fakturze.
/// </summary>
public class InvoiceDetailDto
{
    /// <summary>
    /// Identyfikator faktury.
    /// </summary>
    public int InvoiceId { get; set; }
    
    /// <summary>
    /// Numer faktury.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Data wystawienia faktury.
    /// </summary>
    public DateTime IssueDate { get; set; }
    
    /// <summary>
    /// Termin płatności faktury.
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// Kwota całkowita faktury.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Kwota podatku na fakturze.
    /// </summary>
    public decimal TaxAmount { get; set; }
    
    /// <summary>
    /// Status faktury.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Data płatności faktury.
    /// </summary>
    public DateTime? PaymentDate { get; set; }
    
    /// <summary>
    /// Uwagi do faktury.
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Lista pozycji faktury.
    /// </summary>
    public List<LineItemDto> LineItems { get; set; } = new List<LineItemDto>();
}
