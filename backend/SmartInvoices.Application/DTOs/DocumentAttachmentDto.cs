namespace SmartInvoices.Application.DTOs;

/// <summary>
/// Data Transfer Object dla załącznika dokumentu.
/// </summary>
public class DocumentAttachmentDto
{
    /// <summary>
    /// Identyfikator załącznika.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nazwa pliku.
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Typ MIME pliku.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;
    
    /// <summary>
    /// Rozmiar pliku w bajtach.
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// Data przesłania pliku.
    /// </summary>
    public DateTime UploadedAt { get; set; }
}
