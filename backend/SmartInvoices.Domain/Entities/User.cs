using System;
using System.Collections.Generic;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje użytkownika systemu
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unikalny identyfikator użytkownika
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Adres email użytkownika
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash hasła użytkownika
        /// </summary>
        public string PasswordHash { get;  set; } = string.Empty;

        /// <summary>
        /// Imię użytkownika
        /// </summary>
        public string FirstName { get;  set; } = string.Empty;

        /// <summary>
        /// Nazwisko użytkownika
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Data utworzenia konta użytkownika
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data ostatniego logowania użytkownika
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Określa czy konto użytkownika jest aktywne
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Określa czy użytkownik jest administratorem
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Kolekcja faktur przypisanych do użytkownika
        /// </summary>
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        /// <summary>
        /// Kolekcja wniosków o zmiany złożonych przez użytkownika
        /// </summary>
        public ICollection<ChangeRequest> ChangeRequests { get; set; } = new List<ChangeRequest>();

        /// <summary>
        /// Kolekcja wniosków o zwroty złożonych przez użytkownika
        /// </summary>
        public ICollection<RefundRequest> RefundRequests { get; set; } = new List<RefundRequest>();

        /// <summary>
        /// Ustawienia powiadomień użytkownika
        /// </summary>
        public NotificationSettings NotificationSettings { get; set; } = null!;

        /// <summary>
        /// Kolekcja powiadomień dla użytkownika
        /// </summary>
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
