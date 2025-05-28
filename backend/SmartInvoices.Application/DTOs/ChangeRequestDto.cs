namespace SmartInvoices.Application.DTOs;

/// <summary>
/// Data Transfer Object dla wniosku o zmianę.
/// </summary>
public class ChangeRequestDto
{
    /// <summary>
    /// Identyfikator wniosku o zmianę.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identyfikator faktury, której dotyczy wniosek.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Identyfikatory pozycji faktury, których dotyczy wniosek.
    /// </summary>
    public List<int> LineItemIds { get; set; } = new List<int>();

    /// <summary>
    /// Typ zmiany (np. zmiana ilości, ceny, opisu).
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// Uzasadnienie zmiany.
    /// </summary>
    public string Justification { get; set; } = string.Empty;

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
}
