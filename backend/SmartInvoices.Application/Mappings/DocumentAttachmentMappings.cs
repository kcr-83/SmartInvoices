using SmartInvoices.Application.DTOs;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Mappings;

/// <summary>
/// Klasa pomocnicza do mapowania obiektów DocumentAttachment na DocumentAttachmentDto i odwrotnie.
/// </summary>
public static class DocumentAttachmentMappings
{
    /// <summary>
    /// Mapuje obiekt DocumentAttachment na DocumentAttachmentDto.
    /// </summary>
    /// <param name="attachment">Załącznik do zmapowania</param>
    /// <returns>Zmapowany obiekt DocumentAttachmentDto</returns>
    public static DocumentAttachmentDto ToDto(this DocumentAttachment attachment)
    {
        return new DocumentAttachmentDto
        {
            Id = attachment.AttachmentId,
            FileName = attachment.FileName,
            ContentType = attachment.FileType,
            FileSize = attachment.FileSize,
            UploadedAt = attachment.UploadDate
        };
    }

    /// <summary>
    /// Mapuje listę obiektów DocumentAttachment na listę DocumentAttachmentDto.
    /// </summary>
    /// <param name="attachments">Lista załączników do zmapowania</param>
    /// <returns>Zmapowana lista DocumentAttachmentDto</returns>
    public static List<DocumentAttachmentDto> ToDto(this IEnumerable<DocumentAttachment> attachments)
    {
        return attachments?.Select(attachment => attachment.ToDto()).ToList() ?? new List<DocumentAttachmentDto>();
    }
}
