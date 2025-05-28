namespace SmartInvoices.WebApi.Endpoints.ChangeRequests.GetChangeRequests;

/// <summary>
/// Model odpowiedzi dla pobrania listy wniosków o zmiany.
/// </summary>
public class GetChangeRequestsResponse
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
    public List<ChangeRequestResponse> ChangeRequests { get; set; } = new List<ChangeRequestResponse>();
}

/// <summary>
/// Model wniosku o zmianę w odpowiedzi.
/// </summary>
public class ChangeRequestResponse
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
}
