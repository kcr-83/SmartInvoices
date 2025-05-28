namespace SmartInvoices.Application.DTOs.Invoices;

/// <summary>
/// DTO dla informacji o fakturze.
/// </summary>
public class InvoiceDto
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
}
