using System;
using System.Collections.Generic;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje fakturę w systemie
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Unikalny identyfikator faktury
        /// </summary>
        public int InvoiceId { get; set; }
        
        /// <summary>
        /// Numer faktury
        /// </summary>
        public string InvoiceNumber { get; set; }
        
        /// <summary>
        /// Identyfikator użytkownika, do którego należy faktura
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Data wystawienia faktury
        /// </summary>
        public DateTime IssueDate { get; set; }
        
        /// <summary>
        /// Termin płatności faktury
        /// </summary>
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// Całkowita kwota faktury
        /// </summary>
        public decimal TotalAmount { get; set; }
        
        /// <summary>
        /// Kwota podatku na fakturze
        /// </summary>
        public decimal TaxAmount { get; set; }
        
        /// <summary>
        /// Status faktury
        /// </summary>
        public InvoiceStatus Status { get; set; }
        
        /// <summary>
        /// Data płatności faktury (jeśli opłacona)
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        
        /// <summary>
        /// Dodatkowe uwagi do faktury
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// Data utworzenia faktury w systemie
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Data ostatniej modyfikacji faktury
        /// </summary>
        public DateTime LastModifiedDate { get; set; }
        
        /// <summary>
        /// Referencja do użytkownika, do którego należy faktura
        /// </summary>
        public User User { get; set; }
        
        /// <summary>
        /// Kolekcja pozycji faktury
        /// </summary>
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
        
        /// <summary>
        /// Kolekcja wniosków o zwrot dla tej faktury
        /// </summary>
        public ICollection<RefundRequest> RefundRequests { get; set; } = new List<RefundRequest>();
        
        /// <summary>
        /// Oblicza całkowitą wartość faktury na podstawie pozycji
        /// </summary>
        public void CalculateTotal()
        {
            TotalAmount = 0;
            TaxAmount = 0;
            
            foreach (var item in LineItems)
            {
                TotalAmount += item.TotalPrice;
                TaxAmount += item.Quantity * item.UnitPrice * item.TaxRate / 100;
            }
        }
        
        /// <summary>
        /// Oznacza fakturę jako opłaconą
        /// </summary>
        /// <param name="paymentDate">Data płatności</param>
        public void MarkAsPaid(DateTime paymentDate)
        {
            Status = InvoiceStatus.Paid;
            PaymentDate = paymentDate;
            LastModifiedDate = DateTime.Now;
        }
    }
}
