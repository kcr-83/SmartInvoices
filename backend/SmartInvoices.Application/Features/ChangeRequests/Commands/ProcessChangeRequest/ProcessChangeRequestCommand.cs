// filepath: c:\ProgramData\Comarch IBARD\Sync\f9ac72d3\d955\Praca\Github Copilot\_project\SmartInvoices\backend\SmartInvoices.Application\Features\ChangeRequests\Commands\ProcessChangeRequest\ProcessChangeRequestCommand.cs
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands.ProcessChangeRequest;

/// <summary>
/// Komenda do przetworzenia wniosku o zmianę (akceptacja/odrzucenie).
/// </summary>
public class ProcessChangeRequestCommand : IRequest<Result>
{
    /// <summary>
    /// Identyfikator wniosku o zmianę.
    /// </summary>
    public int ChangeRequestId { get; set; }
    
    /// <summary>
    /// Nowy status wniosku.
    /// </summary>
    public RequestStatus Status { get; set; }
    
    /// <summary>
    /// Komentarz administratora.
    /// </summary>
    public string? Comment { get; set; }
    
    /// <summary>
    /// Identyfikator administratora przetwarzającego wniosek.
    /// </summary>
    public int ProcessedById { get; set; }

    /// <summary>
    /// Konstruktor komendy ProcessChangeRequestCommand.
    /// </summary>
    /// <param name="changeRequestId">Identyfikator wniosku</param>
    /// <param name="status">Nowy status</param>
    /// <param name="processedById">Identyfikator administratora</param>
    /// <param name="comment">Komentarz (opcjonalny)</param>
    public ProcessChangeRequestCommand(int changeRequestId, RequestStatus status, int processedById, string? comment = null)
    {
        ChangeRequestId = changeRequestId;
        Status = status;
        Comment = comment;
        ProcessedById = processedById;
    }
}
