using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.DTOs;

namespace SmartInvoices.Application.Features.Invoices.Queries.GetInvoices;

/// <summary>
/// Zapytanie zwracające listę faktur z możliwością filtrowania i sortowania.
/// </summary>
public class GetInvoicesQuery : IRequest<Result<List<InvoiceDto>>>
{
    /// <summary>
    /// Fraza wyszukiwania do filtrowania faktur.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Data początkowa dla zakresu dat wystawienia faktur.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data końcowa dla zakresu dat wystawienia faktur.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Status faktur do filtrowania.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Minimalna kwota faktury.
    /// </summary>
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// Maksymalna kwota faktury.
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// Pole, według którego mają być sortowane faktury.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Kierunek sortowania (rosnąco/malejąco).
    /// </summary>
    public string? SortDirection { get; set; }

    /// <summary>
    /// Numer strony wyników.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Liczba wyników na stronie.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
