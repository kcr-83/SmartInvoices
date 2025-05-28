using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Interfaces
{
    /// <summary>
    /// Interfejs repozytorium dla wniosków o zwrot.
    /// </summary>
    public interface IRefundRequestRepository
    {
        /// <summary>
        /// Pobiera wniosek o zwrot po ID.
        /// </summary>
        /// <param name="id">ID wniosku o zwrot</param>
        /// <returns>Wniosek o zwrot lub null, jeśli nie znaleziono</returns>
        Task<RefundRequest> GetByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Pobiera listę wniosków o zwrot dla faktury.
        /// </summary>
        /// <param name="invoiceId">ID faktury</param>
        /// <returns>Lista wniosków o zwrot dla faktury</returns>
        Task<IEnumerable<RefundRequest>> GetByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken);

        /// <summary>
        /// Pobiera wszystkie wnioski o zwrot.
        /// </summary>
        /// <returns>Lista wszystkich wniosków o zwrot</returns>
        Task<IEnumerable<RefundRequest>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Dodaje nowy wniosek o zwrot.
        /// </summary>
        /// <param name="refundRequest">Wniosek o zwrot do dodania</param>
        /// <returns>Dodany wniosek o zwrot</returns>
        Task<RefundRequest> AddAsync(RefundRequest refundRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Aktualizuje wniosek o zwrot.
        /// </summary>
        /// <param name="refundRequest">Zaktualizowany wniosek o zwrot</param>
        Task UpdateAsync(RefundRequest refundRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Usuwa wniosek o zwrot.
        /// </summary>
        /// <param name="id">ID wniosku o zwrot do usunięcia</param>
        Task DeleteAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Pobiera wnioski o zwrot według statusu.
        /// </summary>
        /// <param name="status">Status wniosków o zwrot</param>
        /// <returns>Lista wniosków o zwrot o określonym statusie</returns>
        Task<IEnumerable<RefundRequest>> GetByStatusAsync(RefundRequestStatus status);
    }
}
