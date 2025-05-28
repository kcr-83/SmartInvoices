using SmartInvoices.Application.DTOs;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Mappings;

/// <summary>
/// Klasa pomocnicza do mapowania obiektów ChangeRequest na ChangeRequestDto i odwrotnie.
/// </summary>
public static class ChangeRequestMappings
{
    /// <summary>
    /// Mapuje obiekt ChangeRequest na ChangeRequestDto.
    /// </summary>
    /// <param name="changeRequest">Wniosek o zmianę do zmapowania</param>
    /// <returns>Zmapowany obiekt ChangeRequestDto</returns>
    public static ChangeRequestDto ToDto(this ChangeRequest changeRequest)
    {
        return new ChangeRequestDto
        {
            Id = changeRequest.ChangeRequestId,
            InvoiceId = changeRequest.InvoiceId,
            LineItemIds = changeRequest.LineItemIds,
            // ChangeType = changeRequest.ChangeType.ToString(),
            Justification = changeRequest.Reason,
            Status = changeRequest.Status.ToString(),
            CreatedAt = changeRequest.RequestDate,
            RequestedById = changeRequest.ChangeRequestId
        };
    }

    /// <summary>
    /// Mapuje listę obiektów ChangeRequest na listę ChangeRequestDto.
    /// </summary>
    /// <param name="changeRequests">Lista wniosków o zmianę do zmapowania</param>
    /// <returns>Zmapowana lista ChangeRequestDto</returns>
    public static List<ChangeRequestDto> ToDto(this IEnumerable<ChangeRequest> changeRequests)
    {
        return changeRequests?.Select(changeRequest => changeRequest.ToDto()).ToList() ?? new List<ChangeRequestDto>();
    }
}
