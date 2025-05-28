namespace SmartInvoices.Application.DTOs;

/// <summary>
/// Data Transfer Object dla faktury.
/// </summary>
public class InvoiceDto
{
    /// <summary>
    /// Identyfikator faktury.
    /// </summary>
    public int Id { get; set; }
    
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
    /// Całkowita kwota faktury.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Status faktury.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Lista pozycji faktury.
    /// </summary>
    public List<LineItemDto> Items { get; set; } = new List<LineItemDto>();
}
