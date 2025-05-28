using System;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Events
{
    /// <summary>
    /// Zdarzenie domenowe wywoływane po zmianie statusu wniosku
    /// </summary>
    public class RequestStatusChangedEvent
    {
        /// <summary>
        /// Identyfikator wniosku
        /// </summary>
        public int RequestId { get; }
        
        /// <summary>
        /// Typ wniosku
        /// </summary>
        public RequestType RequestType { get; }
        
        /// <summary>
        /// Poprzedni status wniosku
        /// </summary>
        public RequestStatus OldStatus { get; }
        
        /// <summary>
        /// Nowy status wniosku
        /// </summary>
        public RequestStatus NewStatus { get; }
        
        /// <summary>
        /// Identyfikator użytkownika, który zmienił status
        /// </summary>
        public int? ChangedBy { get; }
        
        /// <summary>
        /// Data wystąpienia zdarzenia
        /// </summary>
        public DateTime OccurredOn { get; }
        
        /// <summary>
        /// Wniosek o zmianę (tylko jeśli dotyczy)
        /// </summary>
        public ChangeRequest? ChangeRequest { get; }
        
        /// <summary>
        /// Wniosek o zwrot (tylko jeśli dotyczy)
        /// </summary>
        public RefundRequest? RefundRequest { get; }
        
        /// <summary>
        /// Konstruktor dla wniosku o zmianę
        /// </summary>
        public RequestStatusChangedEvent(
            ChangeRequest request,
            RequestStatus oldStatus,
            int? changedBy = null)
        {
            RequestId = request.ChangeRequestId;
            RequestType = RequestType.Change;
            ChangeRequest = request;
            OldStatus = oldStatus;
            NewStatus = request.Status;
            ChangedBy = changedBy;
            OccurredOn = DateTime.Now;
        }
        
        /// <summary>
        /// Konstruktor dla wniosku o zwrot
        /// </summary>
        public RequestStatusChangedEvent(
            RefundRequest request,
            RequestStatus oldStatus,
            int? changedBy = null)
        {
            RequestId = request.RefundRequestId;
            RequestType = RequestType.Refund;
            RefundRequest = request;
            OldStatus = oldStatus;
            NewStatus = (RequestStatus)(int)request.Status; // Konwersja z RefundRequestStatus na RequestStatus
            ChangedBy = changedBy;
            OccurredOn = DateTime.Now;
        }
    }
}
