using SmartInvoices.Application.DTOs.ChangeRequests;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Application.Interfaces.Repositories;

/// <summary>
/// Interfejs repozytorium wniosków o zmiany.
/// </summary>
public interface IChangeRequestRepository
{
    /// <summary>
    /// Pobiera paginowaną i filtrowaną listę wniosków o zmiany.
    /// </summary>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="page">Numer strony</param>
    /// <param name="pageSize">Rozmiar strony</param>
    /// <param name="status">Status wniosku (opcjonalnie)</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Paginowana lista wniosków o zmiany</returns>
    Task<ChangeRequestListDto> GetChangeRequestsAsync(
        int userId, 
        int page = 1, 
        int pageSize = 10, 
        string? status = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Pobiera szczegóły wniosku o zmianę.
    /// </summary>
    /// <param name="changeRequestId">ID wniosku o zmianę</param>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Szczegóły wniosku o zmianę lub null jeśli nie znaleziono</returns>
    Task<ChangeRequestDetailDto?> GetChangeRequestByIdAsync(
        int changeRequestId, 
        int userId, 
        CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Tworzy nowy wniosek o zmianę.
    /// </summary>
    /// <param name="changeRequest">Dane wniosku o zmianę</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Utworzony wniosek o zmianę</returns>
    Task<ChangeRequest> CreateChangeRequestAsync(
        CreateChangeRequestDto changeRequest, 
        CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Aktualizuje status wniosku o zmianę.
    /// </summary>
    /// <param name="updateData">Dane aktualizacji statusu</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Zaktualizowany wniosek o zmianę lub null jeśli nie znaleziono</returns>
    Task<ChangeRequest?> UpdateStatusAsync(
        UpdateChangeRequestStatusDto updateData, 
        CancellationToken cancellationToken = default);
}
