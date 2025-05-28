namespace SmartInvoices.Application.DTOs;

/// <summary>
/// Data Transfer Object dla wniosku o zwrot.
/// </summary>
public class RefundRequestDto
{
    /// <summary>
    /// Identyfikator wniosku o zwrot.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identyfikator faktury, której dotyczy wniosek.
    /// </summary>
    public int InvoiceId { get; set; }
    
    /// <summary>
    /// Powód zwrotu.
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    
    /// <summary>
    /// Status wniosku.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Data złożenia wniosku.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Identyfikator użytkownika, który złożył wniosek.
    /// </summary>
    public int RequestedById { get; set; }
    
    /// <summary>
    /// Lista załączników do wniosku (np. zdjęcia, dokumenty).
    /// </summary>
    public List<DocumentAttachmentDto> Attachments { get; set; } = new List<DocumentAttachmentDto>();
}
