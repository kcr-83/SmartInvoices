using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Features.Invoices.Queries;
using System.Security.Claims;

namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoicePdf;

/// <summary>
/// Endpoint do pobierania faktury w formacie PDF.
/// </summary>
[Authorize]
public class GetInvoicePdfEndpoint : Endpoint<GetInvoicePdfRequest>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetInvoicePdfEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Konfiguracja endpointu.
    /// </summary>
    public override void Configure()
    {
        Get("/invoices/{invoiceId}/pdf");
        Roles("User", "Admin");
        Description(d =>
        {
            d.WithName("GetInvoicePdf");
            d.WithTags("Invoices", "Invoices.Queries");
            d.WithDescription("Generuje i pobiera fakturę w formacie PDF");
            d.Produces(200, contentType: "application/pdf");
            d.ProducesProblem(401);
            d.ProducesProblem(404);
        });
    }

    /// <summary>
    /// Obsługa żądania pobrania faktury w formacie PDF.
    /// </summary>
    public override async Task HandleAsync(GetInvoicePdfRequest req, CancellationToken ct)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetInvoicePdfQuery
        {
            InvoiceId = req.InvoiceId,
            UserId = userId
        };

        var pdfData = await _mediator.Send<GetInvoicePdfQuery, byte[]?>(query, ct);

        if (pdfData == null || pdfData.Length == 0)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendBytesAsync(pdfData, "application/pdf", $"faktura_{req.InvoiceId}.pdf", cancellation: ct);
    }
}
