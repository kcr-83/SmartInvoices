namespace SmartInvoices.WebApi.Endpoints.ChangeRequests.GetChangeRequests;

/// <summary>
/// Model żądania pobrania listy wniosków o zmiany.
/// </summary>
public class GetChangeRequestsRequest
{
    /// <summary>
    /// Numer strony.
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Rozmiar strony.
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Status wniosku do filtrowania.
    /// </summary>
    public string? Status { get; set; }
}
