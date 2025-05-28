namespace SmartInvoices.Application.DTOs.LineItems;

/// <summary>
/// DTO dla pozycji faktury.
/// </summary>
public class LineItemDto
{
    /// <summary>
    /// Identyfikator pozycji faktury.
    /// </summary>
    public int LineItemId { get; set; }
    
    /// <summary>
    /// Opis pozycji faktury.
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
    /// Stawka podatku VAT.
    /// </summary>
    public decimal TaxRate { get; set; }
    
    /// <summary>
    /// Cena całkowita.
    /// </summary>
    public decimal TotalPrice { get; set; }
}
