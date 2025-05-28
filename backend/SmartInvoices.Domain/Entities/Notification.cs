using System;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje powiadomienia dla użytkowników
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Unikalny identyfikator powiadomienia
        /// </summary>
        public int NotificationId { get; set; }
        
        /// <summary>
        /// Identyfikator użytkownika, do którego należy powiadomienie
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Tytuł powiadomienia
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Treść powiadomienia
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Data utworzenia powiadomienia
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Czy powiadomienie zostało przeczytane
        /// </summary>
        public bool IsRead { get; set; }
        
        /// <summary>
        /// Identyfikator powiązanej encji (np. faktury, wniosku)
        /// </summary>
        public int? RelatedEntityId { get; set; }
        
        /// <summary>
        /// Typ powiązanej encji (np. "Invoice", "ChangeRequest", "RefundRequest")
        /// </summary>
        public string RelatedEntityType { get; set; }
        
        /// <summary>
        /// Użytkownik, do którego należy powiadomienie
        /// </summary>
        public User User { get; set; }
        
        /// <summary>
        /// Oznacza powiadomienie jako przeczytane
        /// </summary>
        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
