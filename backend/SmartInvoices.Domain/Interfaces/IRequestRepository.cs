using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Interfaces
{
    /// <summary>
    /// Interfejs repozytorium do zarządzania wnioskami o zmianę i zwrot
    /// </summary>
    public interface IRequestRepository
    {
        /// <summary>
        /// Pobiera wszystkie wnioski o zmianę
        /// </summary>
        Task<IEnumerable<ChangeRequest>> GetAllChangeRequestsAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wnioski o zmianę danego użytkownika
        /// </summary>
        Task<IEnumerable<ChangeRequest>> GetChangeRequestsByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wnioski o zmianę dla danej pozycji faktury
        /// </summary>
        Task<IEnumerable<ChangeRequest>> GetChangeRequestsByLineItemIdAsync(
            int lineItemId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wnioski o zmianę wg. kryteriów filtrowania
        /// </summary>
        Task<IEnumerable<ChangeRequest>> GetFilteredChangeRequestsAsync(
            int? userId = null,
            int? lineItemId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            RequestStatus? status = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wniosek o zmianę wg. identyfikatora
        /// </summary>
        Task<ChangeRequest> GetChangeRequestByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Dodaje nowy wniosek o zmianę
        /// </summary>
        Task AddChangeRequestAsync(
            ChangeRequest request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Aktualizuje istniejący wniosek o zmianę
        /// </summary>
        Task UpdateChangeRequestAsync(
            ChangeRequest request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Usuwa wniosek o zmianę
        /// </summary>
        Task DeleteChangeRequestAsync(
            ChangeRequest request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wszystkie wnioski o zwrot
        /// </summary>
        Task<IEnumerable<RefundRequest>> GetAllRefundRequestsAsync(
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wnioski o zwrot danego użytkownika
        /// </summary>
        Task<IEnumerable<RefundRequest>> GetRefundRequestsByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wnioski o zwrot dla danej faktury
        /// </summary>
        Task<IEnumerable<RefundRequest>> GetRefundRequestsByInvoiceIdAsync(
            int invoiceId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wnioski o zwrot wg. kryteriów filtrowania
        /// </summary>
        Task<IEnumerable<RefundRequest>> GetFilteredRefundRequestsAsync(
            int? userId = null,
            int? invoiceId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            RequestStatus? status = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera wniosek o zwrot wg. identyfikatora
        /// </summary>
        Task<RefundRequest> GetRefundRequestByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Dodaje nowy wniosek o zwrot
        /// </summary>
        Task AddRefundRequestAsync(
            RefundRequest request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Aktualizuje istniejący wniosek o zwrot
        /// </summary>
        Task UpdateRefundRequestAsync(
            RefundRequest request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Usuwa wniosek o zwrot
        /// </summary>
        Task DeleteRefundRequestAsync(
            RefundRequest request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Dodaje załącznik do wniosku o zwrot
        /// </summary>
        Task AddDocumentAttachmentAsync(
            DocumentAttachment attachment,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Usuwa załącznik z wniosku o zwrot
        /// </summary>
        Task DeleteDocumentAttachmentAsync(
            DocumentAttachment attachment,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Pobiera załączniki dla danego wniosku o zwrot
        /// </summary>
        Task<IEnumerable<DocumentAttachment>> GetDocumentAttachmentsByRefundRequestIdAsync(
            int refundRequestId,
            CancellationToken cancellationToken = default
        );
    }
}
