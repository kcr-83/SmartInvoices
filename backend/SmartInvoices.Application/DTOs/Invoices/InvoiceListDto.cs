namespace SmartInvoices.Application.DTOs.Invoices;

/// <summary>
/// DTO dla listy faktur z informacjami o paginacji.
/// </summary>
public class InvoiceListDto
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
    public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
}
