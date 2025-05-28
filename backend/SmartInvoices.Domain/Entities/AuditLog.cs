using System;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje logi audytowe dla śledzenia zmian w systemie
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// Unikalny identyfikator logu
        /// </summary>
        public int LogId { get; set; }
        
        /// <summary>
        /// Identyfikator użytkownika, który wykonał akcję
        /// </summary>
        public int RefUserId { get; set; }
        
        /// <summary>
        /// Typ akcji (np. "Create", "Update", "Delete", "Approve", "Reject")
        /// </summary>
        public string ActionType { get; private set; } = string.Empty;
        
        /// <summary>
        /// Typ encji, której dotyczy akcja (np. "Invoice", "LineItem", "ChangeRequest")
        /// </summary>
        public string EntityType { get; private set; } = string.Empty;
        
        /// <summary>
        /// Identyfikator encji, której dotyczy akcja
        /// </summary>
        public int EntityId { get; set; }
        
        /// <summary>
        /// Data wykonania akcji
        /// </summary>
        public DateTime ActionDate { get; set; }
        
        /// <summary>
        /// Stara wartość (opcjonalnie, w formacie JSON)
        /// </summary>
        public string OldValue { get; private set; } = string.Empty;
        
        /// <summary>
        /// Nowa wartość (opcjonalnie, w formacie JSON)
        /// </summary>
        public string NewValue { get; private set; } = string.Empty;
        
        /// <summary>
        /// Użytkownik, który wykonał akcję
        /// </summary>
        public User User { get; private set; } = null!;
        
        /// <summary>
        /// Tworzy nowy wpis w logu audytowym
        /// </summary>
        public static AuditLog Create(string actionType, string entityType, int entityId, DateTime timestamp, string? oldValue = null, string? newValue = null, User? user = null)
        {
            return new AuditLog
            {
                ActionType = actionType,
                EntityType = entityType,
                EntityId = entityId,
                ActionDate = timestamp,
                OldValue = oldValue ?? string.Empty,
                NewValue = newValue ?? string.Empty,
                User = user ?? throw new ArgumentNullException(nameof(user))
            };
        }
    }
}
