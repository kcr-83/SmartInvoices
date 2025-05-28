using System;

namespace SmartInvoices.Domain.Enums
{
    /// <summary>
    /// Reprezentuje możliwe statusy wniosków o zwrot w systemie
    /// </summary>
    public enum RefundRequestStatus
    {
        /// <summary>
        /// Wniosek o zwrot utworzony i oczekujący na przetworzenie
        /// </summary>
        Created = 0,
        
        /// <summary>
        /// Wniosek o zwrot w trakcie rozpatrywania
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// Wniosek o zwrot zaakceptowany
        /// </summary>
        Approved = 2,
        
        /// <summary>
        /// Wniosek o zwrot odrzucony
        /// </summary>
        Rejected = 3,
        
        /// <summary>
        /// Wniosek o zwrot anulowany przez użytkownika
        /// </summary>
        Cancelled = 4,
        
        /// <summary>
        /// Wniosek o zwrot wymaga dodatkowych informacji
        /// </summary>
        AdditionalInfoRequired = 5,
        
        /// <summary>
        /// Wniosek o zwrot zrealizowany - środki zwrócone
        /// </summary>
        Refunded = 6,

        /// <summary>
        /// Wniosek o zwrot częściowo zrealizowany
        /// </summary>
        PartiallyRefunded = 7
    }
}
