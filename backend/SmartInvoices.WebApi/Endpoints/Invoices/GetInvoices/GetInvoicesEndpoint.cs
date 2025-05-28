using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Features.Invoices.Queries;
using System.Security.Claims;

namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoices;

/// <summary>
/// Endpoint do pobierania listy faktur.
/// </summary>
[Authorize]
public class GetInvoicesEndpoint : Endpoint<GetInvoicesRequest, GetInvoicesResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetInvoicesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Konfiguracja endpointu.
    /// </summary>
    public override void Configure()
    {
        Get("/invoices");
        Roles("User", "Admin");
        Description(d =>
        {
            d.WithName("GetInvoices");
            d.WithTags("Invoices", "Invoices.Queries");
            d.WithDescription("Zwraca paginowaną listę faktur dla zalogowanego użytkownika z opcją filtrowania");
            d.ProducesProblem(401);
            d.ProducesProblem(403);
        });
    }

    /// <summary>
    /// Obsługa żądania pobrania listy faktur.
    /// </summary>
    public override async Task HandleAsync(GetInvoicesRequest req, CancellationToken ct)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetInvoicesQuery
        {
            UserId = userId,
            Page = req.Page,
            PageSize = req.PageSize,
            Status = req.Status,
            StartDate = req.StartDate,
            EndDate = req.EndDate,
            SortBy = req.SortBy,
            SortOrder = req.SortOrder
        };

        var result = await _mediator.Send<GetInvoicesQuery, Application.DTOs.Invoices.InvoiceListDto>(query, ct);

        var response = new GetInvoicesResponse
        {
            TotalCount = result.TotalCount,
            PageSize = result.PageSize,
            CurrentPage = result.CurrentPage,
            TotalPages = result.TotalPages,
            Invoices = result.Invoices.Select(i => new InvoiceResponse
            {
                InvoiceId = i.InvoiceId,
                InvoiceNumber = i.InvoiceNumber,
                IssueDate = i.IssueDate,
                DueDate = i.DueDate,
                TotalAmount = i.TotalAmount,
                TaxAmount = i.TaxAmount,
                Status = i.Status,
                PaymentDate = i.PaymentDate
            }).ToList()
        };

        await SendOkAsync(response, ct);
    }
}
