// filepath: c:\ProgramData\Comarch IBARD\Sync\f9ac72d3\d955\Praca\Github Copilot\_project\SmartInvoices\backend\SmartInvoices.Application\Features\ChangeRequests\Commands\ProcessChangeRequest\ProcessChangeRequestCommandHandler.cs
using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Common;
using SmartInvoices.Application.Common.Mediator;
using SmartInvoices.Application.Interfaces;
using SmartInvoices.Domain.Enums;
using SmartInvoices.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartInvoices.Application.Features.ChangeRequests.Commands.ProcessChangeRequest;

/// <summary>
/// Handler komendy przetwarzania wniosku o zmianę.
/// </summary>
public class ProcessChangeRequestCommandHandler
    : IRequestHandler<ProcessChangeRequestCommand, Result>
{
    private readonly IChangeRequestRepository _changeRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<ProcessChangeRequestCommandHandler> _logger;

    /// <summary>
    /// Konstruktor handlera ProcessChangeRequestCommandHandler.
    /// </summary>
    /// <param name="changeRequestRepository">Repozytorium wniosków o zmianę</param>
    /// <param name="userRepository">Repozytorium użytkowników</param>
    /// <param name="emailService">Serwis do wysyłania emaili</param>
    /// <param name="logger">Logger</param>
    public ProcessChangeRequestCommandHandler(
        IChangeRequestRepository changeRequestRepository,
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<ProcessChangeRequestCommandHandler> logger
    )
    {
        _changeRequestRepository = changeRequestRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Obsługuje komendę przetwarzania wniosku o zmianę.
    /// </summary>
    /// <param name="request">Komenda przetwarzania wniosku</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Wynik operacji</returns>
    public async Task<Result> Handle(
        ProcessChangeRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Processing change request ID: {ChangeRequestId}, new status: {Status}",
            request.ChangeRequestId,
            request.Status
        );

        try
        {
            // Pobranie wniosku o zmianę
            var changeRequest = await _changeRequestRepository.GetByIdAsync(request.ChangeRequestId);
            
            if (changeRequest == null)
            {
                _logger.LogWarning(
                    "Change request with ID {ChangeRequestId} not found",
                    request.ChangeRequestId
                );
                return Result.Failure(
                    $"Wniosek o zmianę o ID {request.ChangeRequestId} nie został znaleziony."
                );
            }

            // Sprawdzenie, czy wniosek jest w odpowiednim stanie do przetworzenia
            if (changeRequest.Status != RequestStatus.Created)
            {
                _logger.LogWarning(
                    "Change request with ID {ChangeRequestId} is not in Created status",
                    request.ChangeRequestId
                );
                return Result.Failure("Tylko wnioski w stanie oczekującym mogą być przetworzone.");
            }

            // Aktualizacja wniosku
            changeRequest.Status = request.Status;
            changeRequest.AdminNotes = request.Comment ?? string.Empty;
            changeRequest.ReviewedBy = request.ProcessedById;
            changeRequest.ReviewDate = DateTime.UtcNow;

            await _changeRequestRepository.UpdateAsync(changeRequest);

            // Powiadomienie użytkownika o zmianie statusu
            var user = await _userRepository.GetByIdAsync(changeRequest.UserId, cancellationToken);
            if (user != null)
            {
                var statusText =
                    request.Status == RequestStatus.Approved ? "zatwierdzony" : "odrzucony";
                await _emailService.SendEmailAsync(
                    user.Email,
                    $"Status wniosku o zmianę #{changeRequest.ChangeRequestId} został zmieniony",
                    $"Twój wniosek o zmianę #{changeRequest.ChangeRequestId} został {statusText}.\n\n"
                        + (
                            string.IsNullOrEmpty(request.Comment)
                                ? ""
                                : $"Komentarz administratora: {request.Comment}"
                        )
                );
            }

            _logger.LogInformation(
                "Processed change request ID: {ChangeRequestId}, new status: {Status}",
                request.ChangeRequestId,
                request.Status
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing change request ID: {ChangeRequestId}",
                request.ChangeRequestId
            );
            return Result.Failure("Wystąpił błąd podczas przetwarzania wniosku o zmianę.");
        }
    }
}
