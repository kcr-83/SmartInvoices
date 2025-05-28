using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands.CreateChangeRequest;

/// <summary>
/// Komenda do tworzenia nowego wniosku o zmianę.
/// </summary>
public class CreateChangeRequestCommand : IRequest<Result<int>>
{
    /// <summary>
    /// Identyfikator faktury, której dotyczy wniosek.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Identyfikatory pozycji faktury, których dotyczy wniosek.
    /// </summary>
    public List<int> LineItemIds { get; set; } = [];

    /// <summary>
    /// Typ zmiany (np. zmiana ilości, ceny, opisu).
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// Uzasadnienie zmiany.
    /// </summary>
    public string Justification { get; set; } = string.Empty;

    /// <summary>
    /// Identyfikator użytkownika, który tworzy wniosek.
    /// </summary>
    public int RequestedById { get; set; }
}
