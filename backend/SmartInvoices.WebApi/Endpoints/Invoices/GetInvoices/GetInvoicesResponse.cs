namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoices;

/// <summary>
/// Model odpowiedzi dla listy faktur.
/// </summary>
public class GetInvoicesResponse
{
    /// <summary>
    /// Całkowita liczba faktur.
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Rozmiar strony.
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// Numer bieżącej strony.
    /// </summary>
    public int CurrentPage { get; set; }
    
    /// <summary>
    /// Całkowita liczba stron.
    /// </summary>
    public int TotalPages { get; set; }
    
    /// <summary>
    /// Lista faktur.
    /// </summary>
    public List<InvoiceResponse> Invoices { get; set; } = new List<InvoiceResponse>();
}

/// <summary>
/// Model faktury w odpowiedzi.
/// </summary>
public class InvoiceResponse
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
