namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoiceById;

/// <summary>
/// Model odpowiedzi dla szczegółów faktury.
/// </summary>
public class GetInvoiceByIdResponse
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
    public List<LineItemResponse> LineItems { get; set; } = new List<LineItemResponse>();
}

/// <summary>
/// Model pozycji faktury w odpowiedzi.
/// </summary>
public class LineItemResponse
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
