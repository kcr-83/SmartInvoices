using SmartInvoices.Application.DTOs.LineItems;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Mappings;

/// <summary>
/// Klasa pomocnicza do mapowania obiektów LineItem na LineItemDto i odwrotnie.
/// </summary>
public static class LineItemMappings
{
    /// <summary>
    /// Mapuje obiekt LineItem na LineItemDto.
    /// </summary>
    /// <param name="lineItem">Pozycja faktury do zmapowania</param>
    /// <returns>Zmapowany obiekt LineItemDto</returns>
    public static LineItemDto ToDto(this LineItem lineItem) =>
        new LineItemDto
        {
            LineItemId = lineItem.LineItemId,
            Description = lineItem.Description,
            Quantity = lineItem.Quantity,
            UnitPrice = lineItem.UnitPrice,
            TaxRate = lineItem.TaxRate,
            TotalPrice = lineItem.TotalPrice
        };

    /// <summary>
    /// Mapuje listę obiektów LineItem na listę LineItemDto.
    /// </summary>
    /// <param name="lineItems">Lista pozycji faktury do zmapowania</param>
    /// <returns>Zmapowana lista LineItemDto</returns>
    public static List<LineItemDto> ToDto(this IEnumerable<LineItem> lineItems) =>
        lineItems?.Select(lineItem => lineItem.ToDto()).ToList() ?? new List<LineItemDto>();
}
