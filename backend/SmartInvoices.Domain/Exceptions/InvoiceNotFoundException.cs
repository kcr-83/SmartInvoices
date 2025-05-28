using System;

namespace SmartInvoices.Domain.Exceptions
{
    /// <summary>
    /// Wyjątek występujący, gdy faktura nie została znaleziona
    /// </summary>
    public class InvoiceNotFoundException : Exception
    {
        /// <summary>
        /// Identyfikator faktury, która nie została znaleziona
        /// </summary>
        public int InvoiceId { get; }
        
        /// <summary>
        /// Numer faktury (jeśli wyszukiwanie było po numerze faktury)
        /// </summary>
        public string InvoiceNumber { get; }
        
        /// <summary>
        /// Konstruktor z identyfikatorem faktury
        /// </summary>
        /// <param name="invoiceId">Identyfikator faktury, która nie została znaleziona</param>
        public InvoiceNotFoundException(int invoiceId)
            : base($"Faktura o ID {invoiceId} nie została znaleziona.")
        {
            InvoiceId = invoiceId;
        }
        
        /// <summary>
        /// Konstruktor z numerem faktury
        /// </summary>
        /// <param name="invoiceNumber">Numer faktury, która nie została znaleziona</param>
        public InvoiceNotFoundException(string invoiceNumber)
            : base($"Faktura o numerze {invoiceNumber} nie została znaleziona.")
        {
            InvoiceNumber = invoiceNumber;
        }
    }
}
