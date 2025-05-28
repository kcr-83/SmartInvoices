namespace SmartInvoices.Application.DTOs.ChangeRequests;

/// <summary>
/// DTO dla aktualizacji statusu wniosku o zmianę.
/// </summary>
public class UpdateChangeRequestStatusDto
{
    /// <summary>
    /// Identyfikator wniosku o zmianę.
    /// </summary>
    public int ChangeRequestId { get; set; }
    
    /// <summary>
    /// Nowy status wniosku.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Notatki administratora.
    /// </summary>
    public string? AdminNotes { get; set; }
    
    /// <summary>
    /// Identyfikator administratora.
    /// </summary>
    public int AdminId { get; set; }
}
