using System;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje ustawienia powiadomień dla użytkownika
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// Identyfikator ustawień powiadomień
        /// </summary>
        public int NotificationSettingsId { get; set; }
        
        /// <summary>
        /// Identyfikator użytkownika, do którego należą ustawienia powiadomień
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Czy powiadomienia email są włączone
        /// </summary>
        public bool EmailNotificationsEnabled { get; set; } = true;
        
        /// <summary>
        /// Czy powiadomienia w aplikacji są włączone
        /// </summary>
        public bool WebNotificationsEnabled { get; set; }
        
        /// <summary>
        /// Czy powiadomienia o fakturach są włączone
        /// </summary>
        public bool InvoiceNotifications { get; set; } = true;
        
        /// <summary>
        /// Czy powiadomienia o wnioskach o zmianę są włączone
        /// </summary>
        public bool ChangeRequestNotifications { get; set; } = true;
        
        /// <summary>
        /// Czy powiadomienia o wnioskach o zwrot są włączone
        /// </summary>
        public bool RefundRequestNotifications { get; set; } = true;
        
        /// <summary>
        /// Użytkownik, do którego należą ustawienia
        /// </summary>
        public User User { get; private set; } = null!;
        
        /// <summary>
        /// Aktualizuje ustawienia powiadomień
        /// </summary>
        public void UpdateSettings(bool emailEnabled, bool webEnabled, bool invoiceNotifications = true, 
            bool changeRequestNotifications = true, bool refundRequestNotifications = true)
        {
            EmailNotificationsEnabled = emailEnabled;
            WebNotificationsEnabled = webEnabled;
            InvoiceNotifications = invoiceNotifications;
            ChangeRequestNotifications = changeRequestNotifications;
            RefundRequestNotifications = refundRequestNotifications;
        }
    }
}
