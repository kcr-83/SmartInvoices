using SmartInvoices.Application.DTOs;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Mappings;

/// <summary>
/// Klasa pomocnicza do mapowania obiektów RefundRequest na RefundRequestDto i odwrotnie.
/// </summary>
public static class RefundRequestMappings
{
    /// <summary>
    /// Mapuje obiekt RefundRequest na RefundRequestDto.
    /// </summary>
    /// <param name="refundRequest">Wniosek o zwrot do zmapowania</param>
    /// <returns>Zmapowany obiekt RefundRequestDto</returns>
    public static RefundRequestDto ToDto(this RefundRequest refundRequest) =>
        new RefundRequestDto
        {
            Id = refundRequest.RefundRequestId,
            InvoiceId = refundRequest.InvoiceId,
            Reason = refundRequest.Reason,
            Status = refundRequest.Status.ToString(),
            CreatedAt = refundRequest.RequestDate,
            RequestedById = refundRequest.RefundRequestId,
            Attachments =
                refundRequest.DocumentAttachments?.Select(attachment => attachment.ToDto()).ToList()
                ?? new List<DocumentAttachmentDto>()
        };

    /// <summary>
    /// Mapuje listę obiektów RefundRequest na listę RefundRequestDto.
    /// </summary>
    /// <param name="refundRequests">Lista wniosków o zwrot do zmapowania</param>
    /// <returns>Zmapowana lista RefundRequestDto</returns>
    public static List<RefundRequestDto> ToDto(this IEnumerable<RefundRequest> refundRequests) =>
        refundRequests?.Select(refundRequest => refundRequest.ToDto()).ToList()
        ?? new List<RefundRequestDto>();
}
