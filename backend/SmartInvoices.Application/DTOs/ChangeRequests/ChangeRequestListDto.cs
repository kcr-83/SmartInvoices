namespace SmartInvoices.Application.DTOs.ChangeRequests;

/// <summary>
/// DTO dla listy wniosków o zmiany z informacjami o paginacji.
/// </summary>
public class ChangeRequestListDto
{
    /// <summary>
    /// Całkowita liczba wniosków.
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
    /// Lista wniosków o zmiany.
    /// </summary>
    public List<ChangeRequestDto> ChangeRequests { get; set; } = new List<ChangeRequestDto>();
}
