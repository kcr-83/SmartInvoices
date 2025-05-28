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
        public int UserId { get; set; }
        
        /// <summary>
        /// Typ akcji (np. "Create", "Update", "Delete", "Approve", "Reject")
        /// </summary>
        public string ActionType { get; set; }
        
        /// <summary>
        /// Typ encji, której dotyczy akcja (np. "Invoice", "LineItem", "ChangeRequest")
        /// </summary>
        public string EntityType { get; set; }
        
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
        public string OldValue { get; set; }
        
        /// <summary>
        /// Nowa wartość (opcjonalnie, w formacie JSON)
        /// </summary>
        public string NewValue { get; set; }
        
        /// <summary>
        /// Użytkownik, który wykonał akcję
        /// </summary>
        public User User { get; set; }
        
        /// <summary>
        /// Tworzy nowy wpis w logu audytowym
        /// </summary>
        public static AuditLog CreateLogEntry(int userId, string actionType, string entityType, 
            int entityId, string oldValue = null, string newValue = null)
        {
            return new AuditLog
            {
                UserId = userId,
                ActionType = actionType,
                EntityType = entityType,
                EntityId = entityId,
                ActionDate = DateTime.Now,
                OldValue = oldValue,
                NewValue = newValue
            };
        }
    }
}
