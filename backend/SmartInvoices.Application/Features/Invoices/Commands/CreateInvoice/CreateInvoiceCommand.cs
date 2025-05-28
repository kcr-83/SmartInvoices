using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;

namespace SmartInvoices.Application.Features.Invoices.Commands.CreateInvoice;

/// <summary>
/// Komenda do tworzenia nowej faktury.
/// </summary>
public class CreateInvoiceCommand : IRequest<Result<int>>
{
    /// <summary>
    /// Numer faktury.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Data wystawienia faktury.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Termin płatności faktury.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Całkowita kwota faktury.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Lista pozycji faktury.
    /// </summary>
    public List<CreateLineItemDto> Items { get; set; } = new List<CreateLineItemDto>();
}

/// <summary>
/// DTO do tworzenia pozycji faktury.
/// </summary>
public class CreateLineItemDto
{
    /// <summary>
    /// Opis pozycji.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ilość.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Cena jednostkowa.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
