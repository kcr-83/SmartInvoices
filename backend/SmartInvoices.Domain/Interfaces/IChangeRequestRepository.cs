using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums; // Ensure this contains ChangeRequestStatus
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartInvoices.Domain.Interfaces
{
    /// <summary>
    /// Interfejs repozytorium dla wniosków o zmianę.
    /// </summary>
    public interface IChangeRequestRepository
    {
        /// <summary>
        /// Pobiera wniosek o zmianę po ID.
        /// </summary>
        /// <param name="id">ID wniosku o zmianę</param>
        /// <returns>Wniosek o zmianę lub null, jeśli nie znaleziono</returns>
        Task<ChangeRequest> GetByIdAsync(int id);

        /// <summary>
        /// Pobiera listę wniosków o zmianę dla faktury.
        /// </summary>
        /// <param name="invoiceId">ID faktury</param>
        /// <returns>Lista wniosków o zmianę dla faktury</returns>
        Task<IEnumerable<ChangeRequest>> GetByInvoiceIdAsync(int invoiceId);

        /// <summary>
        /// Pobiera wszystkie wnioski o zmianę.
        /// </summary>
        /// <returns>Lista wszystkich wniosków o zmianę</returns>
        Task<IEnumerable<ChangeRequest>> GetAllAsync();

        /// <summary>
        /// Dodaje nowy wniosek o zmianę.
        /// </summary>
        /// <param name="changeRequest">Wniosek o zmianę do dodania</param>
        /// <returns>Dodany wniosek o zmianę</returns>
        Task<ChangeRequest> AddAsync(ChangeRequest changeRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Aktualizuje wniosek o zmianę.
        /// </summary>
        /// <param name="changeRequest">Zaktualizowany wniosek o zmianę</param>
        Task UpdateAsync(ChangeRequest changeRequest);

        /// <summary>
        /// Usuwa wniosek o zmianę.
        /// </summary>
        /// <param name="id">ID wniosku o zmianę do usunięcia</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Pobiera wnioski o zmianę według statusu.
        /// </summary>
        /// <param name="status">Status wniosków o zmianę</param>
        /// <returns>Lista wniosków o zmianę o określonym statusie</returns>
        /// /// Task<IEnumerable<ChangeRequest>> GetByStatusAsync(ChangeRequestStatus status);
    }
}
