using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Features.Invoices.Queries;

namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoiceById;

/// <summary>
/// Endpoint do pobierania szczegółów faktury.
/// </summary>
[Authorize]
public class GetInvoiceByIdEndpoint : Endpoint<GetInvoiceByIdRequest, GetInvoiceByIdResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetInvoiceByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Konfiguracja endpointu.
    /// </summary>
    public override void Configure()
    {
        Get("/invoices/{invoiceId}");
        Roles("User", "Admin");
        Description(d =>
        {
            d.WithName("GetInvoiceById");
            d.WithTags("Invoices", "Invoices.Queries");
            d.WithDescription(
                "Zwraca szczegółowe informacje o konkretnej fakturze wraz z pozycjami"
            );
            d.ProducesProblem(401);
            d.ProducesProblem(404);
        });
    }

    /// <summary>
    /// Obsługa żądania pobrania szczegółów faktury.
    /// </summary>
    public override async Task HandleAsync(GetInvoiceByIdRequest req, CancellationToken ct)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetInvoiceByIdQuery { InvoiceId = req.InvoiceId, UserId = userId };

        var result = await _mediator.Send<
            GetInvoiceByIdQuery,
            Application.DTOs.Invoices.InvoiceDetailDto?
        >(query, ct);

        if (result == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new GetInvoiceByIdResponse
        {
            InvoiceId = result.InvoiceId,
            InvoiceNumber = result.InvoiceNumber,
            IssueDate = result.IssueDate,
            DueDate = result.DueDate,
            TotalAmount = result.TotalAmount,
            TaxAmount = result.TaxAmount,
            Status = result.Status,
            PaymentDate = result.PaymentDate,
            Notes = result.Notes,
            LineItems = result.LineItems
                .Select(
                    li =>
                        new LineItemResponse
                        {
                            LineItemId = li.LineItemId,
                            Description = li.Description,
                            Quantity = li.Quantity,
                            UnitPrice = li.UnitPrice,
                            TaxRate = li.TaxRate,
                            TotalPrice = li.TotalPrice
                        }
                )
                .ToList()
        };

        await SendOkAsync(response, ct);
    }
}
