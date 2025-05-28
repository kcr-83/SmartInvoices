namespace SmartInvoices.Application.DTOs;

/// <summary>
/// Data Transfer Object dla pozycji faktury.
/// </summary>
public class LineItemDto
{
    /// <summary>
    /// Identyfikator pozycji faktury.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identyfikator faktury, do której należy pozycja.
    /// </summary>
    public int InvoiceId { get; set; }
    
    /// <summary>
    /// Opis pozycji.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Ilość.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Cena jednostkowa.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Całkowita kwota pozycji.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
