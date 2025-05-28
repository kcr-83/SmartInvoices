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
        public int RefUserId { get; set; }

        /// <summary>
        /// Tytuł powiadomienia
        /// </summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>
        /// Treść powiadomienia
        /// </summary>
        public string Message { get; private set; } = string.Empty;

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
        public string RelatedEntityType { get; private set; } = string.Empty;

        /// <summary>
        /// Użytkownik, do którego kierowane jest powiadomienie
        /// </summary>
        public User User { get; private set; } = null!;

        /// <summary>
        /// Oznacza powiadomienie jako przeczytane
        /// </summary>
        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
