using System;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Exceptions
{
    /// <summary>
    /// Wyjątek występujący, gdy operacja biznesowa jest nieprawidłowa
    /// </summary>
    public class InvalidOperationException : Exception
    {
        /// <summary>
        /// Konstruktor z wiadomością błędu
        /// </summary>
        /// <param name="message">Wiadomość błędu</param>
        public InvalidOperationException(string message)
            : base(message)
        {
        }
    }
    
    /// <summary>
    /// Wyjątek występujący, gdy operacja na fakturze jest niedozwolona
    /// </summary>
    public class InvalidInvoiceOperationException : InvalidOperationException
    {
        /// <summary>
        /// Identyfikator faktury
        /// </summary>
        public int InvoiceId { get; }
        
        /// <summary>
        /// Status faktury
        /// </summary>
        public InvoiceStatus CurrentStatus { get; }
        
        /// <summary>
        /// Konstruktor z identyfikatorem faktury, statusem i wiadomością
        /// </summary>
        /// <param name="invoiceId">Identyfikator faktury</param>
        /// <param name="status">Status faktury</param>
        /// <param name="message">Wiadomość błędu</param>
        public InvalidInvoiceOperationException(int invoiceId, InvoiceStatus status, string message)
            : base(message)
        {
            InvoiceId = invoiceId;
            CurrentStatus = status;
        }
    }
    
    /// <summary>
    /// Wyjątek występujący, gdy operacja na wniosku jest niedozwolona
    /// </summary>
    public class InvalidRequestOperationException : InvalidOperationException
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
        /// Status wniosku
        /// </summary>
        public RequestStatus CurrentStatus { get; }
        
        /// <summary>
        /// Konstruktor z identyfikatorem wniosku, typem, statusem i wiadomością
        /// </summary>
        /// <param name="requestId">Identyfikator wniosku</param>
        /// <param name="requestType">Typ wniosku</param>
        /// <param name="status">Status wniosku</param>
        /// <param name="message">Wiadomość błędu</param>
        public InvalidRequestOperationException(int requestId, RequestType requestType, RequestStatus status, string message)
            : base(message)
        {
            RequestId = requestId;
            RequestType = requestType;
            CurrentStatus = status;
        }
    }
}
