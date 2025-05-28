using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Features.ChangeRequests.Queries;
using System.Security.Claims;

namespace SmartInvoices.WebApi.Endpoints.ChangeRequests.GetChangeRequests;

/// <summary>
/// Endpoint do pobierania listy wniosków o zmiany.
/// </summary>
[Authorize]
public class GetChangeRequestsEndpoint : Endpoint<GetChangeRequestsRequest, GetChangeRequestsResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public GetChangeRequestsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Konfiguracja endpointu.
    /// </summary>
    public override void Configure()
    {
        Get("/changerequests");
        Roles("User", "Admin");
        Description(d =>
        {
            d.WithName("GetChangeRequests");
            d.WithTags("ChangeRequests", "ChangeRequests.Queries");
            d.WithDescription("Zwraca listę wniosków o zmiany w pozycjach faktur");
            d.ProducesProblem(401);
        });
    }

    /// <summary>
    /// Obsługa żądania pobrania listy wniosków o zmiany.
    /// </summary>
    public override async Task HandleAsync(GetChangeRequestsRequest req, CancellationToken ct)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetChangeRequestsQuery
        {
            UserId = userId,
            Page = req.Page,
            PageSize = req.PageSize,
            Status = req.Status
        };

        var result = await _mediator.Send<GetChangeRequestsQuery, Application.DTOs.ChangeRequests.ChangeRequestListDto>(query, ct);

        var response = new GetChangeRequestsResponse
        {
            TotalCount = result.TotalCount,
            PageSize = result.PageSize,
            CurrentPage = result.CurrentPage,
            TotalPages = result.TotalPages,
            ChangeRequests = result.ChangeRequests.Select(cr => new ChangeRequestResponse
            {
                ChangeRequestId = cr.ChangeRequestId,
                Status = cr.Status,
                RequestDate = cr.RequestDate,
                LineItemId = cr.LineItemId,
                RequestedQuantity = cr.RequestedQuantity,
                RequestedUnitPrice = cr.RequestedUnitPrice,
                RequestedDescription = cr.RequestedDescription,
                Reason = cr.Reason
            }).ToList()
        };

        await SendOkAsync(response, ct);
    }
}
