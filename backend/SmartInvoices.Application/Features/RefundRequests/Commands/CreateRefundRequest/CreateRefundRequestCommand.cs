using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Features.RefundRequests.Commands.CreateRefundRequest;

/// <summary>
/// Komenda do tworzenia nowego wniosku o zwrot.
/// </summary>
public class CreateRefundRequestCommand : IRequest<Result<int>>
{
    /// <summary>
    /// Identyfikator faktury, której dotyczy wniosek.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Powód zwrotu.
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Lista załączników do wniosku.
    /// </summary>
    public List<CreateAttachmentDto> Attachments { get; set; } = new List<CreateAttachmentDto>();

    /// <summary>
    /// Identyfikator użytkownika, który tworzy wniosek.
    /// </summary>
    public int RequestedById { get; set; }
}

/// <summary>
/// DTO do tworzenia załącznika.
/// </summary>
public class CreateAttachmentDto
{
    /// <summary>
    /// Nazwa pliku.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Typ MIME pliku.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Zawartość pliku w postaci bajtów.
    /// </summary>
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
